﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeautifulBlueprints.Layout;

namespace BeautifulBlueprints.Elements
{
    public class Subsection
        : BaseElement
    {
        public string[] SearchParameters { get; private set; }

        public Subsection(
            string name = null,
            decimal minWidth = DEFAULT_MIN_WIDTH,
            decimal? preferredWidth = null,
            decimal maxWidth = DEFAULT_MAX_WIDTH,
            decimal minHeight = DEFAULT_MIN_HEIGHT,
            decimal? preferredHeight = null,
            decimal maxHeight = DEFAULT_MAX_HEIGHT,
            params string[] searchParameters
        )
            : base(name, minWidth, preferredWidth, maxWidth, minHeight, preferredHeight, maxHeight)
        {
            SearchParameters = searchParameters;
        }

        private BaseElement _subsection;
        internal override void Prepare(Solver.SolverOptions options)
        {
            if (options.SubsectionFinder == null)
                _subsection = null;
            else
            {
                _subsection = options.SubsectionFinder(Name, SearchParameters);
                if (_subsection != null)
                    _subsection.Prepare(options);
            }

            base.Prepare(options);
        }

        internal override IEnumerable<Solver.Solution> Solve(decimal left, decimal right, decimal top, decimal bottom)
        {
            if (_subsection == null)
                return new Solver.Solution[0];
            else
                return _subsection.Solve(left, right, top, bottom);
        }

        internal override BaseElementContainer Wrap()
        {
            return new SubsectionContainer(this);
        }
    }

    internal class SubsectionContainer
        : BaseElement.BaseElementContainer
    {
        public string[] Tags { get; set; }

        public SubsectionContainer()
        {
        }

        public SubsectionContainer(Subsection sub)
            : base(sub)
        {
        }

        public override BaseElement Unwrap()
        {
            var s = new Subsection(name: Name,
                minWidth: MinWidth,
                preferredWidth: PreferredWidth,
                maxWidth: MaxWidth,
                minHeight: MinHeight,
                preferredHeight: PreferredHeight,
                maxHeight: MaxHeight,
                searchParameters: Tags
            );

            return s;
        }
    }
}
