using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StatsManager
{
    public int highScore { get; set; }
    public int obsticleP { get; set; }
    public int pickupP { get; set; }
    public int bossDefeated {  get; set; }

    public StatsManager(int hs, int op, int pp, int bd)
    {
        highScore = hs;
        obsticleP = op;
        pickupP = pp;
        bossDefeated = bd;
    }

    public StatsManager() 
    {
        highScore=0;
        obsticleP=0;
        pickupP=0;
        bossDefeated=0;
    }
    public void compareScore(int score)
    {
        if(highScore < score)
        {
            Debug.Log("Tis better score");
        }
    }
}
