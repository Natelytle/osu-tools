// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;

namespace PerformanceCalculatorGUI.Screens.Collections
{
    public partial class AddScoreButton : Container
    {
        private readonly RoundedButton addButton;
        private readonly GridContainer creationContainer;
        private readonly OsuTextBox scoreIdTextBox;
        private readonly OsuTextBox mapIdTextBox;
        private readonly OsuTextBox accTextBox;

        public delegate void OnScoreAddHandler(long scoreId);

        public delegate void OnMapAddHandler(string mapId, double accuracy);

        public event OnScoreAddHandler? OnScoreAdd;
        public event OnMapAddHandler? OnMapAdd;

        private const int height = 40;
        private const int fade_duration = 200;

        public AddScoreButton()
        {
            RelativeSizeAxes = Axes.X;
            Height = height;

            Children = new Drawable[]
            {
                addButton = new RoundedButton
                {
                    RelativeSizeAxes = Axes.X,
                    Text = "+",
                    Height = height,
                    Action = () =>
                    {
                        creationContainer!.FadeIn(fade_duration);
                        addButton!.FadeOut(fade_duration);
                    }
                },
                creationContainer = new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    ColumnDimensions = new[] { new Dimension(), new Dimension(), new Dimension(GridSizeMode.Absolute, 100), new Dimension(GridSizeMode.Absolute, 100) },
                    RowDimensions = new[] { new Dimension(GridSizeMode.AutoSize) },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            scoreIdTextBox = new OsuNumberBox
                            {
                                RelativeSizeAxes = Axes.X,
                                PlaceholderText = "Score ID",
                                CornerRadius = height / 2f
                            },
                            mapIdTextBox = new OsuNumberBox
                            {
                                RelativeSizeAxes = Axes.X,
                                PlaceholderText = "Beatmap ID",
                                CornerRadius = height / 2f
                            },
                            accTextBox = new OsuTextBox
                            {
                                RelativeSizeAxes = Axes.X,
                                PlaceholderText = "100",
                                CornerRadius = height / 2f
                            },
                            new RoundedButton
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = height,
                                Padding = new MarginPadding { Left = 5 },
                                Text = "Add",
                                Action = addScore
                            }
                        }
                    }
                }
            };
        }

        private void addScore()
        {
            if (string.IsNullOrEmpty(scoreIdTextBox.Current.Value) && string.IsNullOrEmpty(mapIdTextBox.Current.Value))
            {
                scoreIdTextBox.FlashColour(ColourInfo.SingleColour(Colour4.Red), 500);
                return;
            }

            creationContainer.FadeOut(fade_duration);
            addButton.FadeIn(fade_duration);

            if (!string.IsNullOrEmpty(scoreIdTextBox.Current.Value))
                OnScoreAdd?.Invoke(long.Parse(scoreIdTextBox.Current.Value));
            else
                OnMapAdd?.Invoke(mapIdTextBox.Current.Value, double.TryParse(accTextBox.Current.Value, out double acc) ? Math.Round(Math.Clamp(acc / 100, 0, 1), 4) : 1);

            scoreIdTextBox.Current.Value = mapIdTextBox.Current.Value = accTextBox.Current.Value = string.Empty;
        }
    }
}
