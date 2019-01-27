using Godot;
using System.Collections.Generic;

public class AudioPlayer : AudioStreamPlayer
{
    [Export]
    public string path = "res://";

    [Export]
    public int samplesArrayIndexFFS = 0;

    //This should work, but no... Godoooooooooot!
    [Export]
    public string[] sampleNames;

    int lastIndex = -1;

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
        if (Playing)
            return;
        
        var sampleNames2 = sampleNamesListFFS_0;

        if (samplesArrayIndexFFS == 1)
            sampleNames2 = sampleNamesListFFS_1;
    
        GD.Print("Samples: "+ sampleNames2.Length);

        int index = 0;

        do
        {
            index = MainController.RandomInt(0, sampleNames2.Length);
        }
        while (index == lastIndex);

        lastIndex = index;
        var name = path + sampleNames2[index];
        Stream = ResourceLoader.Load(name) as AudioStream;
        Play();
    }
    
    

    private void Play()
    {
        Play(0);
    }
}

