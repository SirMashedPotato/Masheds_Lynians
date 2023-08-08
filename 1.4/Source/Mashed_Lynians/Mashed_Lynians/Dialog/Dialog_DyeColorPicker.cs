﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace Mashed_Lynians
{
    /// <summary>
    /// Largely based on Dialog_GlowerColorPicker
    /// </summary>
    public class Dialog_DyeColorPicker : Window
    {
        public override Vector2 InitialSize => new Vector2(360f, 400f);

        public Dialog_DyeColorPicker(Comp_LynianDyeKit dyeComp, Color color, bool primaryColor)
        {
            this.dyeComp = dyeComp;
            this.primaryColor = primaryColor;
            this.color = color;
            oldColor = color;
            r = color.r;
            g = color.g;
            b = color.b;
            forcePause = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
            closeOnAccept = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
            using (TextBlock.Default())
            {
                RectDivider rect = new RectDivider(inRect, 195906069, null);
                HeaderRow(ref rect);
                rect.NewRow(0f, VerticalJustification.Top);
                BottomButtons(ref rect);
                rect.NewRow(0f, VerticalJustification.Bottom);

                Listing_Standard listing_Standard = new Listing_Standard();

                listing_Standard.Begin(rect);
                r = (float)Math.Round(listing_Standard.SliderLabeled("Red (" + r*100 + "%)", r, 0, 1) * 100) / 100;
                g = (float)Math.Round(listing_Standard.SliderLabeled("Green (" + g*100 + "%)", g, 0, 1) * 100) / 100;
                b = (float)Math.Round(listing_Standard.SliderLabeled("Blue (" + b*100 + "%)", b, 0, 1) * 100) / 100;
                listing_Standard.End();
                
                color = new Color(r, g, b, 1);
                rect.NewRow(100f, VerticalJustification.Top);
                ColorReadback(rect, color, oldColor);
            }
        }

        private static void ColorSlider(ref RectDivider layout, ref float value, string label)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(layout);
            value = (int)listing_Standard.SliderLabeled(label+"(" + value + ")", value, 0, 255);
        }

        private static void HeaderRow(ref RectDivider layout)
        {
            using (new TextBlock(GameFont.Medium))
            {
                TaggedString taggedString = "ChooseAColor".Translate().CapitalizeFirst();
                RectDivider rect = layout.NewRow(Text.CalcHeight(taggedString, layout.Rect.width), VerticalJustification.Top);
                Widgets.Label(rect, taggedString);
            }
        }

        private void BottomButtons(ref RectDivider layout)
        {
            RectDivider rectDivider = layout.NewRow(ButSize.y, VerticalJustification.Bottom);
            if (Widgets.ButtonText(rectDivider.NewCol(ButSize.x, HorizontalJustification.Left), "Cancel".Translate(), true, true, true, null))
            {
                Close(true);
            }
            if (Widgets.ButtonText(rectDivider.NewCol(ButSize.x, HorizontalJustification.Right), "Accept".Translate(), true, true, true, null))
            {
                color.a = 1;
                if (primaryColor)
                {
                    dyeComp.primaryColor = color;
                } 
                else
                {
                    dyeComp.secondaryColor = color;
                }
               
                Close(true);
            }
        }

        private static void ColorReadback(Rect rect, Color color, Color oldColor)
        {
            rect.SplitVertically((rect.width - 26f) / 2f, out Rect parent, out Rect parent2);
            RectDivider rectDivider = new RectDivider(parent, 195906069, null);
            TaggedString label = "CurrentColor".Translate().CapitalizeFirst();
            TaggedString label2 = "OldColor".Translate().CapitalizeFirst();
            float width = Mathf.Max(new float[]
            {
                100f,
                label.GetWidthCached(),
                label2.GetWidthCached()
            });
            RectDivider rect2 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            Widgets.Label(rect2.NewCol(width, HorizontalJustification.Left), label);
            Widgets.DrawBoxSolid(rect2, color);
            RectDivider rect3 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            Widgets.Label(rect3.NewCol(width, HorizontalJustification.Left), label2);
            Widgets.DrawBoxSolid(rect3, oldColor);
            RectDivider rect4 = new RectDivider(parent2, 195906069, null);
            rect4.NewCol(26f, HorizontalJustification.Left);
        }

        private float r;
        private float g;
        private float b;

        private bool primaryColor;
        private Color color;
        private Color oldColor;
        private Comp_LynianDyeKit dyeComp;

        ///UI shite
        protected static readonly Vector2 ButSize = new Vector2(150f, 38f);
    }
}