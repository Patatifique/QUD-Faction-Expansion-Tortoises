#nullable disable
namespace XRL
{
  [HasModSensitiveStaticCache]
  public static class Sibs_Tortoises_Achievement
  {
    public static readonly AchievementInfo SIBS_TORTOISES_TORTOISES_ENDING = new AchievementInfo(
      "ACH_SIBS_TORTOISES_TORTOISES_ENDING", 
      "FE - Tortoises : Shell of a friend", 
      "welcome.png", 
      "do not keep that name I beg of you.");

    public static readonly AchievementInfo SIBS_TORTOISES_POACHER_ENDING = new AchievementInfo(
      "ACH_SIBS_TORTOISES_POACHER_ENDING", 
      "FE - Tortoises : Crack the Saltback's Shell", 
      "welcome.png", 
      "Witness the ruins of a once peacefull Saltback Graveyard.");

    [ModSensitiveCacheInit]
    public static void Init()
    {
      // had to reload the achievements json because it reads it too soon otherwise
      AchievementManager.Load();
    }
  }
}