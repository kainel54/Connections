using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CSVParser : MonoBehaviour
{
    [SerializeField] private string FILE_NAME;
    private static string FILE_PATH = "Assets/0_Data";
    [SerializeField] private string OUTPUT_PATH = "Assets/0_Data"; // 나올 파일 경로
    private int _startIndex = 4;

    static string SPLIT = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

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

                string path = "Assets/08SO/InHae/SkillData/BaseStatSO/Generic";

                string attackDamageSkillStat = $"{path}/AttackDamageSkillStat.asset";
                string attackCountSkillStat = $"{path}/AttackCountSkillStat.asset";
                string coolTimeStat = $"{path}/CoolTimeSkillStat.asset";
                string reShootTimeStat = $"{path}/ReShootTimeSkillStat.asset";
                string criticalChanceStat = $"{path}/CriticalChanceSkillStat.asset";
                string skillActiveDurationStat = $"{path}/ActiveDurationSkillStat.asset";

                DefaultSkillStatInfoSO attackDamageSkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(attackDamageSkillStat);
                DefaultSkillStatInfoSO attackCountSkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(attackCountSkillStat);
                DefaultSkillStatInfoSO coolTimeStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(coolTimeStat);
                DefaultSkillStatInfoSO reShootTimeStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(reShootTimeStat);
                DefaultSkillStatInfoSO criticalChanceStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(criticalChanceStat);
                DefaultSkillStatInfoSO skillActiveDurationStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(skillActiveDurationStat);

                AssetDatabase.CreateAsset(genericData, genericPath);

                int damage = int.Parse(data[i]["Damage"].ToString());
                float cooltime = float.Parse(data[i]["Cooldown"].ToString());
                string attackTypeString = data[i]["AttackType"].ToString();

                genericData.fieldType = SkillFieldDataType.Generic;
                if (!Enum.TryParse(attackTypeString, true, out SkillAttackType skillAttackType))
                {
                    print($"TryParse failed {skillAttackType.ToString()}");
                    skillAttackType = SkillAttackType.Range;
                }
                genericData.attackType = skillAttackType;

                genericData.attackDamageStat.statInfo = attackDamageSkillStatSO;
                genericData.attackDamageStat.Defaultvalue = damage;

                genericData.attackCountStat.statInfo = attackCountSkillStatSO;
                genericData.attackCountStat.Defaultvalue = 1;

                genericData.coolTimeStat.statInfo = coolTimeStatSO;
                genericData.coolTimeStat.Defaultvalue = cooltime;

                genericData.reShootTimeStat.statInfo = reShootTimeStatSO;
                genericData.reShootTimeStat.Defaultvalue = 1;

                genericData.criticalChanceStat.statInfo = criticalChanceStatSO;
                genericData.criticalChanceStat.Defaultvalue = 0;

                genericData.skillActiveDurationStat.statInfo = skillActiveDurationStatSO;
                genericData.skillActiveDurationStat.Defaultvalue = 0;

                genericData.skillDamageDelay = 0;
                genericData.skillActiveDelay = 4;
            }

            if (AssetDatabase.LoadAssetAtPath<ProjectileSkillDataSO>(projectilePath) == null)
            {
                ProjectileSkillDataSO projectileData = ScriptableObject.CreateInstance<ProjectileSkillDataSO>();

                string path = "Assets/08SO/InHae/SkillData/BaseStatSO/Projectile";

                string projectileCountSkillStat = $"{path}/ProjectileCountSkillStat.asset";
                string projectileMoveSpeedSkillStat = $"{path}/ProjectileMoveSpeedSkillStat.asset";
                string projectilePenetrationSkillStat = $"{path}/ProjectilePenetrationSkillStat.asset";
                string projectileReflectionSkillStat = $"{path}/ProjectileReflectionSkillStat.asset";
                string projectileTrajectorySkillStat = $"{path}/ProjectileTrajectorySkillStat.asset";

                DefaultSkillStatInfoSO projectileCountSkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(projectileCountSkillStat);
                DefaultSkillStatInfoSO projectileMoveSpeedSkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(projectileMoveSpeedSkillStat);
                DefaultSkillStatInfoSO projectilePenetrationSkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(projectilePenetrationSkillStat);
                DefaultSkillStatInfoSO projectileReflectionSkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(projectileReflectionSkillStat);
                DefaultSkillStatInfoSO projectileTrajectorySkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(projectileTrajectorySkillStat);

                AssetDatabase.CreateAsset(projectileData, projectilePath);

                // csv 데이터 가져오기

                projectileData.fieldType = SkillFieldDataType.Projectile;

                projectileData.projectileCountStat.statInfo = projectileCountSkillStatSO;
                projectileData.projectileCountStat.Defaultvalue = 1;

                projectileData.projectileMoveSpeedStat.statInfo = projectileMoveSpeedSkillStatSO;
                projectileData.projectileMoveSpeedStat.Defaultvalue = 1;

                projectileData.projectilePenetrationCountStat.statInfo = projectilePenetrationSkillStatSO;
                projectileData.projectilePenetrationCountStat.Defaultvalue = 1;

                projectileData.projectileReflectionCountStat.statInfo = projectileReflectionSkillStatSO;
                projectileData.projectileReflectionCountStat.Defaultvalue = 1;

                projectileData.projectileTrajectoryStat.statInfo = projectileTrajectorySkillStatSO;
            }

            if (AssetDatabase.LoadAssetAtPath<RangeSkillDataSO>(rangePath) == null)
            {
                RangeSkillDataSO rangeData = ScriptableObject.CreateInstance<RangeSkillDataSO>();

                string path = "Assets/08SO/InHae/SkillData/BaseStatSO/Range";

                string rangeObjSkillStat = $"{path}/RangeObjSkillStat.asset";
                string rangeAttackSizeSkillStat = $"{path}/RangeAttackSizeSkillStat.asset";

                DefaultSkillStatInfoSO rangeObjSkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(rangeObjSkillStat);
                DefaultSkillStatInfoSO rangeAttackSizeSkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(rangeAttackSizeSkillStat);

                AssetDatabase.CreateAsset(rangeData, rangePath);

                rangeData.fieldType = SkillFieldDataType.Range;

                rangeData.rangeObjCountStat.statInfo = rangeObjSkillStatSO;
                rangeData.rangeObjCountStat.Defaultvalue = 1;

                rangeData.rangeAttackSizeStat.statInfo = rangeAttackSizeSkillStatSO;
                rangeData.rangeAttackSizeStat.attackType = RangeSkillAttackType.Sphere; // 나중에 수정
                rangeData.rangeAttackSizeStat.DefaultSphereValue = 1; // 값 받아서 수정
                rangeData.rangeAttackSizeStat.DefaultWidthValue = 1;  // 값 받아서 수정
                rangeData.rangeAttackSizeStat.DefaultHeightValue = 1; // 값 받아서 수정
            }

            if (AssetDatabase.LoadAssetAtPath<TargetingSkillDataSO>(targetingPath) == null)
            {
                TargetingSkillDataSO targetingData = ScriptableObject.CreateInstance<TargetingSkillDataSO>();

                string path = "Assets/08SO/InHae/SkillData/BaseStatSO/Targeting";

                string canSkillRangeSkillStat = $"{path}/CanSkillRangeSkillStat.asset";

                DefaultSkillStatInfoSO canSkillRangeSkillStatSO = AssetDatabase.LoadAssetAtPath<DefaultSkillStatInfoSO>(canSkillRangeSkillStat);

                AssetDatabase.CreateAsset(targetingData, targetingPath);

                targetingData.fieldType = SkillFieldDataType.Targeting;

                targetingData.canUseSkillRangeStat.statInfo = canSkillRangeSkillStatSO;
                targetingData.canUseSkillRangeStat.Defaultvalue = 0;
            }

            AssetDatabase.SaveAssets();
        }

        AssetDatabase.Refresh();
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

        return list; // 나중에 바꾸기
    }
}