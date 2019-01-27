using Godot;
using System;

public class AllySpawner : Node
{
    Timer timer;
    Random r;
    PackedScene allyScene;
    
    [Export]
    Curve spawnDelay;

    [Export]
    float roundDuration = 60;
    [Export]
    float baseSpawnDelay = 10;

    int startTime;

    public override void _Ready()
    {
        allyScene = ResourceLoader.Load("Scenes/AllyFighter.tscn") as PackedScene;

        r = new Random();
        timer = new Timer();
        timer.Connect("timeout", this, "OnSpawnTimer");
        timer.WaitTime = 2;
        AddChild(timer);
        timer.Start();

        startTime = OS.GetSystemTimeSecs();
    }

    public void OnSpawnTimer()
    {
        if (MainController.I.mothership == null)
        {
            timer.Stop();
            return;
        }

        timer.WaitTime = spawnDelay.Interpolate((OS.GetSystemTimeSecs() - startTime) / roundDuration) * baseSpawnDelay;
        GD.Print( timer.WaitTime);
        
        timer.Start();

        GD.Print("Spawn ally");
        var ally = allyScene.Instance() as KinematicBody;
        GetParent().AddChild(ally);
        ally.Translation = MainController.I.mothership.Translation + new Vector3(-1.7f,1.1f,0);
        ally.Rotate(Vector3.Up, Mathf.Pi);
        MainController.I.fighters.Add((ShipMain)ally);

        var rallyPoint = (MainController.I.mothership.Translation + (Vector3.Back * 15)) + MainController.RandomPointOnSphere() * 5;
        ((ShipMain)ally).MoveTo(rallyPoint);
    }
}
