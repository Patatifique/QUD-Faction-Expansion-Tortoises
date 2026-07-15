#nullable disable
namespace XRL
{
  [HasModSensitiveStaticCache]
  public static class Sibs_Tortoises_Achievement
  {
    public static readonly AchievementInfo SIBS_TORTOISES_TORTOISES_ENDING = new AchievementInfo(
      "ACH_SIBS_TORTOISES_TORTOISES_ENDING", 
      "FE - Tortoises: Shell Of A Friend", 
      "welcome.png", 
      "Protect the sanctity of the Saltback Graveyard.");

    public static readonly AchievementInfo SIBS_TORTOISES_POACHER_ENDING = new AchievementInfo(
      "ACH_SIBS_TORTOISES_POACHER_ENDING", 
      "FE - Tortoises: Cracking The Saltback Shell", 
      "welcome.png", 
      "Witness the ruin of the once peaceful Saltback Graveyard.");

    [ModSensitiveCacheInit]
    public static void Init()
    {
      // had to reload the achievements json because it reads it too soon otherwise
      AchievementManager.Load();
    }
  }
}