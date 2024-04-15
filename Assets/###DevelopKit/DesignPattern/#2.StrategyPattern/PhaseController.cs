using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseController : Singleton<PhaseController>
{
    private Dictionary<string, Phase> phaseDic;
    private Phase currentPhase;

    void Start()
    {
        phaseDic = new Dictionary<string, Phase>
        {
            { "InitPhase", new InitPhase() },
            { "PlayerPhase", new PlayerPhase() },
            { "EnemyPhase", new EnemyPhase() }
        };

        SetCurrentPhase("InitPhase");
        StartPhase();
    }

    public void StartPhase()
    {
        if (IsEmptyCurrentPhase())
            return;

        currentPhase.StartPhase();
    }

    void Update()
    {
        if (IsEmptyCurrentPhase())
            return;

        currentPhase.UpdatePhase();
    }

    public void EndPhase()
    {
        if (IsEmptyCurrentPhase())
            return;

        currentPhase.EndPhase();
    }

    private void SetCurrentPhase(string phaseName) => currentPhase = phaseDic[phaseName];

    private bool IsEmptyCurrentPhase()
    {
        if (currentPhase == null)
        {
            Debug.LogError("CurrentPhase is null");
            return true;
        }
        return false;
    }
}
