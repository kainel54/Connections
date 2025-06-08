using UnityEngine;

namespace YH.StatSystem
{
    public static class DamageCalculator
    {
        public static int DamageCalculate(float damageCoefficient, EntityStat dealer, EntityStat target, string mainStatName, out bool isCritical)
        {
            float damage = 0;
            damage.AddStatCalculate(dealer, mainStatName);
            damage.ProportionCalculate(dealer, mainStatName, target, "Defense");

            if (isCritical = IsCritical(dealer))
                damage.PercentStatCalculate(dealer, "CriticalDamage");

            damage *= damageCoefficient / 100;

            return Mathf.CeilToInt(damage);
        }

        private static bool IsCritical(EntityStat dealer)
        {
            //ũ��Ƽ������ Ȯ��
            bool isCritical = false;
            if (dealer.TryGetElement("Critical", out StatElement critical))
            {
                float rand = Random.Range(0, 100);
                if (rand < critical.Value) isCritical = true;
            }
            return isCritical;
        }
        private static void AddStatCalculate(this ref float amount, EntityStat statCompo, string statName)
        {
            //��������
            if (statCompo.TryGetElement(statName, out StatElement stat))
            {
                amount += stat.Value;
            }
        }
        private static void SubtractStatCalculate(this ref float amount, EntityStat statCompo, string statName)
        {
            //��������
            if (statCompo.TryGetElement(statName, out StatElement stat))
            {
                amount -= stat.Value;
            }
        }
        private static void PercentStatCalculate(this ref float amount, EntityStat statCompo, string statName)
        {
            //n% ���� �� �̷���
            if (statCompo.TryGetElement(statName, out StatElement stat))
            {
                amount *= stat.Value / 100;
            }
        }
        private static void ProportionCalculate(this ref float amount, EntityStat numeratorStatCompo, string numerator,
            EntityStat denominatorStatCompo, string denominator, float logAmount = 10)
        {
            //numerator�� denominator�� ���̰� logAmount�谡 ���� 2������
            //ex) numerator: 1000, denominator: 100, logAmount: 10�϶� 2�谡
            //ex) numerator: 100, denominator: 10000, logAmount: 10�϶� 1/4��
            //�������� ���� �ΰ��� ���÷��̼��� ����
            if (numeratorStatCompo.TryGetElement(numerator, out StatElement numeratorStat) &&
                denominatorStatCompo.TryGetElement(denominator, out StatElement denominatorStat) &&
                denominatorStat.Value != 0)
            {
                amount *= Mathf.Log(numeratorStat.Value / denominatorStat.Value * 10 + 1, logAmount);
            }
        }
    }
}
