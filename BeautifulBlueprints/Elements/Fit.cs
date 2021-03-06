﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BeautifulBlueprints.Layout;

namespace BeautifulBlueprints.Elements
{
    public class Fit
        : BaseContainerElement
    {
        internal const Orientation DEFAULT_ORIENTATION = Orientation.Horizontal;
        internal const bool DEFAULT_MINIMIZE_REPEATS = true;
        internal const bool DEFAULT_ALLOW_ZERO_REPEATS = true;

        private readonly Orientation _orientation;
        public Orientation Orientation { get { return _orientation; } }

        private readonly bool _minimizeRepeats;
        public bool MinimizeRepeats { get { return _minimizeRepeats; } }

        private readonly bool _allowZeroRepeats;
        public bool AllowZeroRepeats { get { return _allowZeroRepeats; } }

        public Fit(
            bool minimizeRepeats = DEFAULT_MINIMIZE_REPEATS,
            bool allowZeroRepeats = DEFAULT_ALLOW_ZERO_REPEATS,
            Orientation orientation = DEFAULT_ORIENTATION,
            string name = null,
            decimal minWidth = DEFAULT_MIN_WIDTH,
            decimal? preferredWidth = null,
            decimal maxWidth = DEFAULT_MAX_WIDTH,
            decimal minHeight = DEFAULT_MIN_HEIGHT,
            decimal? preferredHeight = null,
            decimal maxHeight = DEFAULT_MAX_HEIGHT
        )
            : base(name, minWidth, preferredWidth, maxWidth, minHeight, preferredHeight, maxHeight)

        {
            _minimizeRepeats = minimizeRepeats;
            _allowZeroRepeats = allowZeroRepeats;
            _orientation = orientation;
        }

        internal override IEnumerable<Solver.Solution> Solve(decimal left, decimal right, decimal top, decimal bottom)
        {
            List<Solver.Solution> solutions = new List<Solver.Solution>();

            var self = FillSpace(left, right, top, bottom);
            solutions.Add(self);

            //Early exit for no children case
            var child = Children.SingleOrDefault();
            if (child == null)
                return solutions;

            

            //How many whole fits can we squeeze in at the minimum extent?
            int repeatCount;

            if (MinimizeRepeats)
            {
                //How few elements can we fit, if we stretch them as much as possible?
                //How many whole elements can we fit, if we squeeze them up as much as possible?
                var maxExtent = (Orientation == Orientation.Horizontal ? child.MaxWidth : child.MaxHeight);
                if (maxExtent.IsEqualTo(0))
                    throw new LayoutFailureException("Repeat cannot contain an element with 0 maximum size", this);

                repeatCount = (int)Math.Ceiling((Orientation == Orientation.Horizontal ? (self.Right - self.Left) : (self.Top - self.Bottom)) / maxExtent);
            }
            else
            {
                //How many whole elements can we fit, if we squeeze them up as much as possible?
                var minExtent = (Orientation == Orientation.Horizontal ? child.MinWidth : child.MinHeight);
                if (minExtent.IsEqualTo(0))
                    throw new LayoutFailureException("Repeat cannot contain an element with 0 minimum size", this);

                repeatCount = (int)Math.Floor((Orientation == Orientation.Horizontal ? (self.Right - self.Left) : (self.Top - self.Bottom)) / minExtent);
            }

            //Bail out if there are zero repeats
            if (repeatCount == 0 && !AllowZeroRepeats)
                throw new LayoutFailureException("Repeat element repeats zero times, but \"AllowZeroRepeats\" is false", this);
            else if (repeatCount == 0)
                return solutions;

            //Solve children
            solutions.AddRange(Repeat.LayoutRepeats((uint) repeatCount, child, Orientation, self.Left, self.Right, self.Top, self.Bottom));

            return solutions;
        }

        protected override int MaximumChildren
        {
            get
            {
                return 1;
            }
        }

        internal override BaseElementContainer Wrap()
        {
            return new FitContainer(this);
        }
    }

    internal class FitContainer
        : BaseContainerElement.BaseContainerElementContainer
    {
        [DefaultValue(Fit.DEFAULT_ORIENTATION)]
        public Orientation Orientation { get; set; }

        [DefaultValue(Fit.DEFAULT_MINIMIZE_REPEATS)]
        public bool MinimizeRepeats { get; set; }

        [DefaultValue(Fit.DEFAULT_ALLOW_ZERO_REPEATS)]
        public bool AllowZeroRepeats { get; set; }

        public FitContainer()
        {
        }

        public FitContainer(Fit split)
            : base(split)
        {
            Orientation = split.Orientation;
            MinimizeRepeats = split.MinimizeRepeats;
            AllowZeroRepeats = split.AllowZeroRepeats;
        }

        public override BaseElement Unwrap()
        {
            return UnwrapChildren(new Fit(
                minimizeRepeats: MinimizeRepeats,
                allowZeroRepeats: AllowZeroRepeats,
                orientation: Orientation,
                name: Name,
                minWidth: MinWidth,
                preferredWidth: PreferredWidth,
                maxWidth: MaxWidth,
                minHeight: MinHeight,
                preferredHeight: PreferredHeight,
                maxHeight: MaxHeight
            ));
        }
    }
}
