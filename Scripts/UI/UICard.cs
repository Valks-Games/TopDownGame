namespace RTS;

public partial class UICard : MarginContainer
{
    PanelContainer panelContainer;
    int totalCards;
    int columns;

    public void PreInit(int totalCards, int columns)
    {
        this.totalCards = totalCards;
        this.columns = columns;
    }

    public override void _Ready()
    {
        panelContainer = GetNode<PanelContainer>("PanelContainer");

        UpdateSize();

        GetTree().Root.SizeChanged += () =>
        {
            UpdateSize();
        };
    }

    void UpdateSize()
    {
        var windowSize = DisplayServer.WindowGetSize();
        var winFactorX = 4.5f;
        var winFactorY = 1.75f;
        var cardFactor = 4.0f;


        if (totalCards > 4)
        {
            panelContainer.CustomMinimumSize =
                new Vector2
                (
                    x: windowSize.X / winFactorX,
                    y: windowSize.Y / (winFactorY * 1.2f)
                );
        }
        else
        {
            panelContainer.CustomMinimumSize =
                new Vector2
                (
                    x: windowSize.X / winFactorX,
                    y: windowSize.Y / winFactorY
                );
        }
        

    }
}
