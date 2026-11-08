// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Humanizer;
using osu.Game.Rulesets.Mania.Difficulty;

namespace PerformanceCalculatorGUI.Configuration
{
    public static class OsuDifficultyTuningParameters
    {
        public static readonly IReadOnlyList<DifficultyTuningSection<ManiaDifficultyConstants>> Sections = new[]
        {
            new DifficultyTuningSection<ManiaDifficultyConstants>("Performance scales",
                doubleParam(nameof(ManiaDifficultyConstants.StarRatingExponent), c => c.StarRatingExponent, (c, v) => c with { StarRatingExponent = v }, defaultEnabled: false),
                doubleParam(nameof(ManiaDifficultyConstants.PerformanceMultiplier), c => c.PerformanceMultiplier, (c, v) => c with { PerformanceMultiplier = v }, defaultEnabled: false),
                doubleParam(nameof(ManiaDifficultyConstants.PerformanceExponent), c => c.PerformanceExponent, (c, v) => c with { PerformanceExponent = v }, defaultEnabled: false)
            ),
            new DifficultyTuningSection<ManiaDifficultyConstants>("Skill Multipliers",
                doubleParam(nameof(ManiaDifficultyConstants.CoordinationMultiplier), c => c.CoordinationMultiplier, (c, v) => c with { CoordinationMultiplier = v }),
                doubleParam(nameof(ManiaDifficultyConstants.JackMultiplier), c => c.JackMultiplier, (c, v) => c with { JackMultiplier = v }),
                doubleParam(nameof(ManiaDifficultyConstants.ReleaseMultiplier), c => c.ReleaseMultiplier, (c, v) => c with { ReleaseMultiplier = v }),
                doubleParam(nameof(ManiaDifficultyConstants.SpeedMultiplier), c => c.SpeedMultiplier, (c, v) => c with { SpeedMultiplier = v }),
                doubleParam(nameof(ManiaDifficultyConstants.TechnicalMultiplier), c => c.TechnicalMultiplier, (c, v) => c with { TechnicalMultiplier = v })
            ),
            new DifficultyTuningSection<ManiaDifficultyConstants>("Skill Accuracy Curves",
                doubleParam(nameof(ManiaDifficultyConstants.CoordinationAccuracyScaleFactor), c => c.CoordinationAccuracyScaleFactor, (c, v) => c with { CoordinationAccuracyScaleFactor = v }),
                doubleParam(nameof(ManiaDifficultyConstants.JackAccuracyScaleFactor), c => c.JackAccuracyScaleFactor, (c, v) => c with { JackAccuracyScaleFactor = v }),
                doubleParam(nameof(ManiaDifficultyConstants.ReleaseAccuracyScaleFactor), c => c.ReleaseAccuracyScaleFactor, (c, v) => c with { ReleaseAccuracyScaleFactor = v }),
                doubleParam(nameof(ManiaDifficultyConstants.SpeedAccuracyScaleFactor), c => c.SpeedAccuracyScaleFactor, (c, v) => c with { SpeedAccuracyScaleFactor = v }),
                doubleParam(nameof(ManiaDifficultyConstants.TechnicalAccuracyScaleFactor), c => c.TechnicalAccuracyScaleFactor, (c, v) => c with { TechnicalAccuracyScaleFactor = v })
            ),
            new DifficultyTuningSection<ManiaDifficultyConstants>("Skill Strain Decay",
                doubleParam(nameof(ManiaDifficultyConstants.CoordinationStrainDecayBase), c => c.CoordinationStrainDecayBase, (c, v) => c with { CoordinationStrainDecayBase = v }),
                doubleParam(nameof(ManiaDifficultyConstants.JackStrainDecayBase), c => c.JackStrainDecayBase, (c, v) => c with { JackStrainDecayBase = v }),
                doubleParam(nameof(ManiaDifficultyConstants.ReleaseStrainDecayBase), c => c.ReleaseStrainDecayBase, (c, v) => c with { ReleaseStrainDecayBase = v }),
                doubleParam(nameof(ManiaDifficultyConstants.SpeedStrainDecayBase), c => c.SpeedStrainDecayBase, (c, v) => c with { SpeedStrainDecayBase = v }),
                doubleParam(nameof(ManiaDifficultyConstants.TechnicalStrainDecayBase), c => c.TechnicalStrainDecayBase, (c, v) => c with { TechnicalStrainDecayBase = v })
            ),
        };

        public static readonly IReadOnlyList<DifficultyTuningParameter<ManiaDifficultyConstants>> All =
            Sections.SelectMany(s => s.Parameters).ToArray();

        /// <summary>
        /// Returns property names from <see cref="OsuDifficultyConstants"/> that are not covered by any tuning parameter definition.
        /// Useful as a drift detection warning when the constants record gains new properties.
        /// </summary>
        public static IReadOnlyList<string> GetUncoveredProperties()
        {
            var definedNames = new HashSet<string>(All.Select(p => p.PropertyName));

            return typeof(ManiaDifficultyConstants)
                   .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   .Where(p => p.CanRead && !definedNames.Contains(p.Name))
                   .Select(p => p.Name)
                   .ToList();
        }

        private static DifficultyTuningParameter<ManiaDifficultyConstants> doubleParam(
            string propertyName, Func<ManiaDifficultyConstants, double> getter,
            Func<ManiaDifficultyConstants, double, ManiaDifficultyConstants> setter,
            bool defaultEnabled = true, double minValue = 0.01, double? maxValue = null)
        {
            return DifficultyTuningParameter<ManiaDifficultyConstants>.ForDouble(
                propertyName, propertyName.Humanize(LetterCasing.Sentence), getter, setter, defaultEnabled, minValue, maxValue);
        }

        private static DifficultyTuningParameter<ManiaDifficultyConstants> intParam(
            string propertyName, Func<ManiaDifficultyConstants, int> getter,
            Func<ManiaDifficultyConstants, int, ManiaDifficultyConstants> setter,
            bool defaultEnabled = true, int minValue = 1, int? maxValue = null)
        {
            return DifficultyTuningParameter<ManiaDifficultyConstants>.ForInt(
                propertyName, propertyName.Humanize(LetterCasing.Sentence), getter, setter, defaultEnabled, minValue, maxValue);
        }
    }
}
