using Godot;
using System;
using System.Collections.Generic;

public class MainController : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public static MainController I;

    public KinematicBody mothership;
    public static Random r = new Random();

    public List<ShipMain> enemies = new List<ShipMain>();
    public List<ShipMain> fighters = new List<ShipMain>();
    public List<ShipMain> selectedFighters = new List<ShipMain>();

    CPUParticles explosion;
    Timer timer;


    [Export]
    float startDelay = 30f;

    [Signal]
    public delegate void start();


    [Signal]
    public delegate void OnTargetDestroyed();
    

    [Signal]
    public delegate void OnHalf();
    
    [Signal]
    public delegate void OnCritical();

    public override void _EnterTree()
    {
        I = this;

        mothership = GetNode("Mothership") as KinematicBody;
        var hp = mothership.GetNode("HP") as HP;

        hp.Connect("OnDeath", this, "OnMothershipDeath");
        hp.Connect("OnHPChanged", this, "IntegrityCheck");

        //fighters.Add(GetNode<ShipMain>("AllyFighter"));

        explosion = GetNode("Explosion") as CPUParticles;


    }

    public void OnGameStartTimer()
    {
        EmitSignal("start");
        
    }

    public void OnMothershipDeath()
    {
        explosion.Emitting = true;
        mothership = null;

        foreach (var e in enemies)
        {

            e.MoveTo(MainController.RandomPointOnSphere() * 200000);
        }

        foreach (var e in fighters)
        {
            e.MoveTo(MainController.RandomPointOnSphere() * 200000);
        }

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        timer = new Timer();
        timer.OneShot = true;
        timer.Connect("timeout", this, "OnGameStartTimer");
        timer.WaitTime = startDelay;
        AddChild(timer);
        timer.Start();
    }

    int oldValue = 100;

    public void IntegrityCheck(int hp) 
    {
        if (hp < 50 && oldValue >= 50) 
        {
            EmitSignal("OnHalf");
        }
        else if (hp < 15 && oldValue >= 15) 
        {
            EmitSignal("OnCritical");
        }

        oldValue = hp;
    }

    public static int RandomInt(int min, int max)
    {
        return r.Next(min, max);
    }

    public static float RandomFloat(float min, float max)
    {
        return min + (float)r.NextDouble() * (max - min);
    }

    public static Vector3 RandomPointOnSphere()
    {

        var randomAngle = RandomFloat(0, Mathf.Pi * 2);
        return new Vector3(Mathf.Cos(randomAngle), RandomFloat(-1, 1), Mathf.Sin(randomAngle));
    }

    public void AddEnemy(ShipMain enemy)
    {
        enemies.Add(enemy);
        var hp = enemy.GetNode("HP") as HP;
        GD.Print("EnemyHP: " + hp);
        hp.Connect("OnDeath", this, "OnEnemyDeath");
    }

    void OnEnemyDeath()
    {
        GD.Print("EnemyDestroyed");
        EmitSignal("OnTargetDestroyed");
    }
}
