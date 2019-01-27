using Godot;

public class HP : Node
{
    [Export]
    float startHP = 100;

    [Signal]
    public delegate void OnDeath();

    float hp;
    
    public float GetHP() {
    	return hp;
    }

    public override void _Ready()
    {
        hp = startHP;
    }

    public void LoseHP(float amount)
    {
        if (hp > 0)
        {
            hp -= amount;

            if (hp <= 0)
            {
                GD.Print("OnDeath");
                EmitSignal("OnDeath");
            }
        }
    }
}
