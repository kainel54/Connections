using UnityEngine;

namespace YH.StatSystem
{
    public static class DamageCalculator
    {
        public static int DamageCalculate(float damageCoefficient, StatCompo dealer, StatCompo target, string mainStatName, out bool isCritical)
        {
            float damage = 0;
            damage.AddStatCalculate(dealer, mainStatName);
            damage.ProportionCalculate(dealer, mainStatName, target, "Defense");

            if (isCritical = IsCritical(dealer))
                damage.PercentStatCalculate(dealer, "CriticalDamage");

            damage *= damageCoefficient / 100;

            return Mathf.CeilToInt(damage);
        }

        private static bool IsCritical(StatCompo dealer)
        {
            //크리티컬인지 확인
            bool isCritical = false;
            if (dealer.TryGetElement("Critical", out StatElement critical))
            {
                float rand = Random.Range(0, 100);
                if (rand < critical.Value) isCritical = true;
            }
            return isCritical;
        }
        private static void AddStatCalculate(this ref float amount, StatCompo statCompo, string statName)
        {
            //순수증가
            if (statCompo.TryGetElement(statName, out StatElement stat))
            {
                amount += stat.Value;
            }
        }
        private static void SubtractStatCalculate(this ref float amount, StatCompo statCompo, string statName)
        {
            //순수증가
            if (statCompo.TryGetElement(statName, out StatElement stat))
            {
                amount -= stat.Value;
            }
        }
        private static void PercentStatCalculate(this ref float amount, StatCompo statCompo, string statName)
        {
            //n% 증가 뭐 이런거
            if (statCompo.TryGetElement(statName, out StatElement stat))
            {
                amount *= stat.Value / 100;
            }
        }
        private static void ProportionCalculate(this ref float amount, StatCompo numeratorStatCompo, string numerator,
            StatCompo denominatorStatCompo, string denominator, float logAmount = 10)
        {
            //numerator와 denominator의 차이가 logAmount배가 날때 2배차이
            //ex) numerator: 1000, denominator: 100, logAmount: 10일때 2배가
            //ex) numerator: 100, denominator: 10000, logAmount: 10일때 1/4배
            //방어력으로 인한 민감한 인플레이션을 방지
            if (numeratorStatCompo.TryGetElement(numerator, out StatElement numeratorStat) &&
                denominatorStatCompo.TryGetElement(denominator, out StatElement denominatorStat) &&
                denominatorStat.Value != 0)
            {
                amount *= Mathf.Log(numeratorStat.Value / denominatorStat.Value * 10 + 1, logAmount);
            }
        }
    }
}
