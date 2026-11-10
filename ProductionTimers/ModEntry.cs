using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;

namespace ProductionTimers;

internal sealed class ModEntry : Mod
{
    private const float BackgroundOpacity = 0.58f;
    private const int HorizontalPadding = 5;
    private const int VerticalPadding = 2;

    public override void Entry(IModHelper helper)
    {
        helper.Events.Display.RenderedWorld += OnRenderedWorld;
    }

    private void OnRenderedWorld(object? sender, RenderedWorldEventArgs e)
    {
        GameLocation? location = Game1.currentLocation;
        if (location is null || Game1.eventUp || Game1.currentMinigame is not null)
            return;

        Rectangle viewport = Game1.viewport;
        SpriteBatch spriteBatch = e.SpriteBatch;

        // Only inspect the current location's placed objects, and draw only objects
        // whose tile bounds intersect the camera viewport.
        foreach (KeyValuePair<Vector2, StardewValley.Object> pair in location.Objects.Pairs)
        {
            StardewValley.Object machine = pair.Value;
            int remaining = machine.MinutesUntilReady;

            // Completed/ready machines and machines which aren't processing have no timer.
            if (remaining <= 0 || machine.readyForHarvest)
                continue;

            Vector2 worldTopLeft = pair.Key * 64f;
            Rectangle worldBounds = new(
                (int)worldTopLeft.X,
                (int)worldTopLeft.Y,
                64,
                64
            );

            if (!viewport.Intersects(worldBounds))
                continue;

            string label = FormatTime(remaining);
            Vector2 textSize = Game1.smallFont.MeasureString(label);

            float screenCenterX = worldTopLeft.X + 32f - viewport.X;
            float screenY = worldTopLeft.Y - viewport.Y - textSize.Y - 7f;

            // Keep labels inside the visible screen if a machine is at the top edge.
            screenY = Math.Max(2f, screenY);

            Vector2 textPosition = new(
                screenCenterX - textSize.X / 2f,
                screenY
            );

            Rectangle background = new(
                (int)MathF.Floor(textPosition.X - HorizontalPadding),
                (int)MathF.Floor(textPosition.Y - VerticalPadding),
                (int)MathF.Ceiling(textSize.X + HorizontalPadding * 2),
                (int)MathF.Ceiling(textSize.Y + VerticalPadding * 2)
            );

            spriteBatch.Draw(
                Game1.staminaRect,
                background,
                Color.Black * BackgroundOpacity
            );

            spriteBatch.DrawString(
                Game1.smallFont,
                label,
                textPosition,
                Color.White
            );
        }
    }

    private static string FormatTime(int totalMinutes)
    {
        if (totalMinutes >= 60)
        {
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            return minutes == 0 ? $"{hours}h" : $"{hours}h {minutes}m";
        }

        return $"{totalMinutes}m";
    }
}
