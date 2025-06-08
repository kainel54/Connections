namespace ObjectPooling
{
    public enum ObjectType
    {
        Coin, SoundPlayer, SkillDropObj, PartDropObj, NodeAbilityDropObj, HealingPotion
    }
    public enum ProjectileType
    {
        Bullet, Grenade, EnemyEnergyBall, EnemyFireBall, LightningBeam, SpinksSlice, SandTornado, FallMagic, FallAttack

    }
    public enum EnemyPoolingType
    {
        //Melee
        DefalutEnemy, SelfBombEnemy, ShieldEnemy, DashEnemy, SelfBombEnemy_BigBig,
        //Ranged
        WizardEnemy, ShamanEnemy, ThrowEnemy, ThrowEnemy_Division, Wizard_Beam,
    }
    public enum UIPoolingType
    {
        BombCircleDisplay, DamageText, UIPopUpText, BombBoxDisplay, RingDisplay, CircleRingDisplay, TutorialPanel
    }
    public enum EffectPoolingType
    {
        Explosion, BulletImpact, EnergyBallImpact, MuzzleFlash, ShamanEnemyCircle, FallMagicImpact, SpawnNotice,
        PartNodeEquipEffect, DefenceBuff, DamageBuff, HealingBuff, DebuffEffectCircle
    }

    public enum PlayerSkillProjectileType
    {
        EnergyBall, Arrow,
    }

    public enum PlayerSkillProjectileEffectType
    {
        EnergyBallImpact, ArrowImpact,
    }
}