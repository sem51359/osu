﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets;

namespace osu.Game.Overlays.KeyBinding
{
    public class RestorableKeyBindingRow : Container, IFilterable
    {
        private readonly object key;
        private readonly ICollection<DatabasedKeyBinding> bindings;
        public readonly KeyBindingRow KeyBindingRow;

        private bool matchingFilter;

        public bool MatchingFilter
        {
            get => matchingFilter;
            set
            {
                matchingFilter = value;
                this.FadeTo(!matchingFilter ? 0 : 1);
            }
        }

        public bool FilteringActive { get; set; }

        public IEnumerable<string> FilterTerms => bindings.Select(b => b.KeyCombination.ReadableString()).Prepend(key.ToString());

        public RestorableKeyBindingRow(object key, ICollection<DatabasedKeyBinding> bindings, RulesetInfo ruleset, IEnumerable<KeyCombination> defaults)
        {
            this.key = key;
            this.bindings = bindings;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding { Right = SettingsPanel.CONTENT_MARGINS };

            InternalChildren = new Drawable[]
            {
                new RestoreDefaultValueButton<bool>
                {
                    Current = KeyBindingRow.IsDefault,
                    Action = () => { KeyBindingRow.RestoreDefaults(); }
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding { Left = SettingsPanel.CONTENT_MARGINS },
                    Child = KeyBindingRow = new KeyBindingRow(key, bindings.Where(b => ((int)b.Action).Equals((int)key)))
                    {
                        AllowMainMouseButtons = ruleset != null,
                        Defaults = defaults
                    }
                },
            };
        }
    }
}
