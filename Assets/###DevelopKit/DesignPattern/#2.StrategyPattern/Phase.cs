using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Phase
{
    public string PhaseName;
    public string PhaseDescription;

    public virtual void Init() 
    { 
        PhaseName = string.Empty;
        PhaseDescription = string.Empty;
    }

    public abstract void StartPhase();

    public abstract void UpdatePhase();

    public abstract void EndPhase();
}

public class InitPhase : Phase
{
    public override void Init()
    {
        PhaseName = "InitPhase";
        PhaseDescription = "This phase inits for stage";
    }

    public override void StartPhase()
    {
        Debug.Log("스테이지 생성 시작");
    }

    public override void UpdatePhase()
    {
        
    }

    public override void EndPhase()
    {
        Debug.Log("스테이지 생성 완료");
    }
}

public class PlayerPhase : Phase
{
    public override void Init()
    {
        PhaseName = "PlayerPhase";
        PhaseDescription = "Player has a turn at This phase";
    }

    public override void StartPhase()
    {
        Debug.Log("플레이어턴 시작");
    }

    public override void UpdatePhase()
    {

    }

    public override void EndPhase()
    {
        Debug.Log("플레이어턴 종료");
    }
}

public class EnemyPhase : Phase
{
    public override void Init()
    {
        PhaseName = "EnemyPhase";
        PhaseDescription = "Enemy has a turn at This phase";
    }

    public override void StartPhase()
    {
        Debug.Log("적턴 시작");
    }

    public override void UpdatePhase()
    {

    }

    public override void EndPhase()
    {
        Debug.Log("적턴 종료");
    }
}