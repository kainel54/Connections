using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CSVParser : MonoBehaviour
{
#if UNITY_EDITOR
    
    [SerializeField] private string FILE_NAME;
    private static string FILE_PATH = "Assets/0_Data";
    [SerializeField] private string OUTPUT_PATH = "Assets/0_Data"; // 나올 파일 경로
    private int _startIndex = 4;
    
    static string SPLIT = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };
    
    private readonly string originPath = "Assets/08SO/InHae/SkillData/BaseStatSO";
    private readonly string[] genericPaths = {
        "AttackDamageSkillStat",
        "AttackCountSkillStat",
        "CoolTimeSkillStat",
        "ReShootTimeSkillStat",
        "CriticalChanceSkillStat",
        "ActiveDurationSkillStat"
    };
    
    private readonly string[] projectilePaths =
    {
        "ProjectileCountSkillStat",
        "ProjectileMoveSpeedSkillStat",
        "ProjectilePenetrationSkillStat",
        "ProjectileReflectionSkillStat",
        "ProjectileTrajectorySkillStat"
    };
    
    private readonly string[] rangePaths =
    {
        "RangeObjCountSkillStat",
        "RangeAttackSizeSkillStat"
    };
    
    private readonly string[] targetingPaths =
    {
        "CanSkillRangeSkillStat"
    };
    
    private enum SkillType
    {
        Generic,
        Projectile,
        Range,
        Targeting
    }
    
    private Dictionary<Enum, string> typeDic = new Dictionary<Enum, string>()
    {
        { SkillType.Generic, "Generic" },
        { SkillType.Projectile, "Projectile" },
        { SkillType.Range, "Range" },
        { SkillType.Targeting, "Targeting" },
    };
    
    [ContextMenu("READ")]
    private void Read()
    {
        List<Dictionary<string, object>> data = CSVParser.ReadCSV(FILE_NAME);
    
        for (int i = 0; i < data.Count; ++i)
        {
            if (i < _startIndex) continue;
    
            string scriptName = data[i]["Script"].ToString();
            string fullPath = $"{OUTPUT_PATH}/{i - 3}.{scriptName}";
    
            if (!Directory.Exists(fullPath))
            {
                print($"<color=red>Create directory {fullPath}</color>");
                Directory.CreateDirectory(fullPath);
            }
    
            // so 생성
            string genericPath = $"{fullPath}/{scriptName}GenericData.asset";
            string projectilePath = $"{fullPath}/{scriptName}ProjectileData.asset";
            string rangePath = $"{fullPath}/{scriptName}RangeData.asset";
            string targetingPath = $"{fullPath}/{scriptName}TargetingData.asset";
    
            if (AssetDatabase.LoadAssetAtPath<GenericSkillDataSO>(genericPath) == null)
            {
                GenericSkillDataSO genericData = ScriptableObject.CreateInstance<GenericSkillDataSO>();
    
                List<DefaultSkillStatInfoSO> defaultSkillStatInfoSOs = new();
    
                for (int j = 0; j < genericPaths.Length; ++j)
                {
                    DefaultSkillStatInfoSO so = CreateSO(SkillType.Generic, genericPaths[j]);
                    defaultSkillStatInfoSOs.Add(so);
                }
    
                genericData.Setup(defaultSkillStatInfoSOs);
    
                AssetDatabase.CreateAsset(genericData, genericPath);
    
                string attackTypeString = data[i]["AttackType"].ToString();
                if (!Enum.TryParse(attackTypeString, true, out SkillAttackType skillAttackType))
                {
                    print($"TryParse failed {skillAttackType.ToString()}");
                    skillAttackType = SkillAttackType.Range;
                }
                int damage = int.Parse(data[i]["Damage"].ToString());
                float cooltime = float.Parse(data[i]["Cooldown"].ToString());
    
                genericData.fieldType = SkillFieldDataType.Generic;
                genericData.attackType = skillAttackType;
                genericData.attackDamageStat.Defaultvalue = damage;
                genericData.attackCountStat.Defaultvalue = 1;
                genericData.coolTimeStat.Defaultvalue = cooltime;
                genericData.reShootTimeStat.Defaultvalue = 1;
                genericData.criticalChanceStat.Defaultvalue = 0;
                genericData.skillActiveDurationStat.Defaultvalue = 0;
                genericData.skillDamageDelay = 0;
                genericData.skillActiveDelay = 4;
    
                EditorUtility.SetDirty(genericData);
            }
    
            if (AssetDatabase.LoadAssetAtPath<ProjectileSkillDataSO>(projectilePath) == null)
            {
                ProjectileSkillDataSO projectileData = ScriptableObject.CreateInstance<ProjectileSkillDataSO>();
    
                List<DefaultSkillStatInfoSO> defaultSkillStatInfoSOs = new();
    
                for (int j = 0; j < projectilePaths.Length; ++j)
                {
                    DefaultSkillStatInfoSO so = CreateSO(SkillType.Projectile, projectilePaths[j]);
                    defaultSkillStatInfoSOs.Add(so);
                }
    
                projectileData.Setup(defaultSkillStatInfoSOs);
    
                AssetDatabase.CreateAsset(projectileData, projectilePath);
    
                // csv 데이터 가져오기
    
                projectileData.fieldType = SkillFieldDataType.Projectile;
                projectileData.projectileCountStat.Defaultvalue = 1;
                projectileData.projectileMoveSpeedStat.Defaultvalue = 1;
                projectileData.projectilePenetrationCountStat.Defaultvalue = 1;
                projectileData.projectileReflectionCountStat.Defaultvalue = 1;
    
                EditorUtility.SetDirty(projectileData);
            }
    
            if (AssetDatabase.LoadAssetAtPath<RangeSkillDataSO>(rangePath) == null)
            {
                RangeSkillDataSO rangeData = ScriptableObject.CreateInstance<RangeSkillDataSO>();
    
                List<DefaultSkillStatInfoSO> defaultSkillStatInfoSOs = new();
    
                for (int j = 0; j < rangePaths.Length; ++j)
                {
                    DefaultSkillStatInfoSO so = CreateSO(SkillType.Range, rangePaths[j]);
                    defaultSkillStatInfoSOs.Add(so);
                }
    
                rangeData.Setup(defaultSkillStatInfoSOs);
    
                AssetDatabase.CreateAsset(rangeData, rangePath);
    
                //
    
                rangeData.fieldType = SkillFieldDataType.Range;
                rangeData.rangeObjCountStat.Defaultvalue = 1;
                rangeData.rangeAttackSizeStat.attackType = RangeSkillAttackType.Sphere; // 나중에 수정
                rangeData.rangeAttackSizeStat.SphereDefaultValue = 1; // 값 받아서 수정
                rangeData.rangeAttackSizeStat.WidthDefaultValue = 1;  // 값 받아서 수정
                rangeData.rangeAttackSizeStat.HeightDefaultValue = 1; // 값 받아서 수정
    
                EditorUtility.SetDirty(rangeData);
            }
    
            if (AssetDatabase.LoadAssetAtPath<TargetingSkillDataSO>(targetingPath) == null)
            {
                TargetingSkillDataSO targetingData = ScriptableObject.CreateInstance<TargetingSkillDataSO>();
    
                List<DefaultSkillStatInfoSO> defaultSkillStatInfoSOs = new();
    
                for (int j = 0; j < targetingPaths.Length; ++j)
                {
                    DefaultSkillStatInfoSO so = CreateSO(SkillType.Targeting, targetingPaths[j]);
                    defaultSkillStatInfoSOs.Add(so);
                }
    
                targetingData.Setup(defaultSkillStatInfoSOs);
    
                AssetDatabase.CreateAsset(targetingData, targetingPath);
    
                targetingData.fieldType = SkillFieldDataType.Targeting;
                targetingData.canUseSkillRangeStat.Defaultvalue = 0;
    
                EditorUtility.SetDirty(targetingData);
            }
        }
    
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private DefaultSkillStatInfoSO CreateSO(SkillType skillType, string path)
    {
        string targetPath = $"{originPath}/{typeDic[skillType]}/{path}.asset";
        return AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(targetPath);
    }
    
    private static List<Dictionary<string, object>> ReadCSV(string fileName)
    {
        List<Dictionary<string, object>> list = new();
        TextAsset targetFile = AssetDatabase.LoadAssetAtPath<TextAsset>($"{FILE_PATH}/{fileName}.csv");
    
        string[] lines = Regex.Split(targetFile.text, LINE_SPLIT);
    
        if (lines.Length <= 1) return list; // 한 줄밖에 없으면 그냥 반환해도 됨
    
        string[] header = Regex.Split(lines[0], SPLIT);
    
        for (int i = 0; i < lines.Length; ++i)
        {
            string[] values = Regex.Split(lines[i], SPLIT);
    
            Dictionary<string, object> entry = new();
    
            for (int j = 0; j < header.Length; ++j)
            {
                string value = j < values.Length && values[j].Length > 0 ? values[j] : "";
    
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
    
                object finalValue;
                int intValue;
                float floatValue;
    
                if (int.TryParse(value, out intValue))
                {
                    finalValue = intValue;
                }
                else if (float.TryParse(value, out floatValue))
                {
                    finalValue = floatValue;
                }
                else
                    finalValue = value;
    
                entry[header[j]] = finalValue;
            }
    
            list.Add(entry);
        }
    
        return list;
    }
#endif
}