using Godot;
using System.Collections.Generic;

public class AudioPlayer : AudioStreamPlayer2D
{
    [Export]
    public string path = "res://";

    [Export]
    public int samplesArrayIndexFFS = 0;

    //This should work, but no... Godoooooooooot!
    [Export]
    public string[] sampleNames;

    string[] sampleNamesListFFS_0
     =
        {
            "Affirmative.ogg",
            "AssigningAttackVector.ogg",
            "TangoInSights.ogg",
            "TargetLockedIn.ogg",
            "TaskReceived.ogg",
            "OnIt.ogg"
        };
        
       string[] sampleNamesListFFS_1 =
        {
            "HostileNeutralized.ogg",
            "ScratchOneBogey.ogg",
            "TangoDown.ogg",
            "TargetDestroyed.ogg",
            "TaskCompleted.ogg",
            "ThreatRemoved.ogg"
        };

    public override void _Ready()
    {
    }

    public void PlayRandom()
    {
    
        var sampleNames2 = sampleNamesListFFS_0;

        if (samplesArrayIndexFFS == 1)
            sampleNames2 = sampleNamesListFFS_1;
    
        GD.Print("Samples: "+ sampleNames2.Length);
        var name = path + sampleNames2[MainController.RandomInt(0, sampleNames2.Length)];
        Stream = ResourceLoader.Load(name) as AudioStream;
        Play();
    }
}
