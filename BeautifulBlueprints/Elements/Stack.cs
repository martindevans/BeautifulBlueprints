﻿using System.ComponentModel;

namespace BeautifulBlueprints.Elements
{
    public class Stack
        : BaseElement
    {
        [DefaultValue(Orientation.Horizontal)]
        public Orientation Orientation { get; set; }

        [DefaultValue(HorizontalAlignment.Center)]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [DefaultValue(VerticalAlignment.Bottom)]
        public VerticalAlignment VerticalAlignment { get; set; }

        [DefaultValue(Spacing.Uniform)]
        public Spacing Spacing { get; set; }

        public Stack(
            float minWidth = 0,
            float maxWidth = float.PositiveInfinity,
            float minHeight = 0,
            float maxHeight = float.PositiveInfinity,
            Margin?margin= null,
            Orientation orientation = Orientation.Horizontal,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment verticalAlignment = VerticalAlignment.Bottom,
            Spacing spacing = Spacing.Uniform
        )
            : base(minWidth, maxWidth, minHeight, maxHeight, margin)
        {
            Orientation = orientation;
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
            Spacing = spacing;
        }

        public Stack()
            : this(minWidth: 0)
        {
        }

        protected override bool AllowChildren
        {
            get
            {
                return true;
            }
        }

        internal override System.Collections.Generic.IEnumerable<Layout.Solver.Solution> Solve(float left, float right, float top, float bottom)
        {
            throw new System.NotImplementedException();
        }
    }
}
