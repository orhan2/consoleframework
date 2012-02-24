﻿using System.Collections.Generic;
using ConsoleFramework.Core;
using ConsoleFramework.Native;

namespace ConsoleFramework.Controls
{
    /// <summary>
    /// Контрол, который может состоять из других контролов.
    /// Позиционирует входящие в него контролы в соответствии с внутренним поведением панели и
    /// заданными свойствами дочерних контролов.
    /// Как и все контролы, связан с виртуальным канвасом.
    /// Может быть самым первым контролом программы (окно не может, к примеру, оно может существовать
    /// только в рамках хоста окон).
    /// </summary>
    public class Panel : Control {
        public Panel() {
        }

        public Panel(Control parent) : base(parent) {
        }

        public CHAR_ATTRIBUTES Background {
            get;
            set;
        }

        public new void AddChild(Control control) {
            base.AddChild(control);
        }

        /// <summary>
        /// Размещает элементы вертикально, самым простым методом.
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize) {
            int totalHeight = 0;
            int maxWidth = 0;
            foreach (Control child in children) {
                child.Measure(new Size(int.MaxValue, int.MaxValue));
                totalHeight += child.DesiredSize.Height;
                if (child.DesiredSize.Width > maxWidth) {
                    maxWidth = child.DesiredSize.Width;
                }
            }
            return new Size(maxWidth, totalHeight);
        }
        
        protected override Size ArrangeOverride(Size finalSize) {
            int totalHeight = 0;
            foreach (Control child in children) {
                int y = totalHeight;
                int width = child.DesiredSize.Width;
                int height = child.DesiredSize.Height;
                child.Arrange(new Rect(0, y, width, height));
                totalHeight += height;
            }
            return finalSize;
        }

        /// <summary>
        /// Рисует исключительно себя - просто фон.
        /// </summary>
        /// <param name="buffer"></param>
        public override void Render(RenderingBuffer buffer) {
            for (int x = 0; x < ActualWidth; ++x) {
                for (int y = 0; y < ActualHeight; ++y) {
                    buffer.SetPixel(x + ActualOffset.X, y + ActualOffset.Y, 'x', CHAR_ATTRIBUTES.BACKGROUND_BLUE |
                        CHAR_ATTRIBUTES.BACKGROUND_GREEN | CHAR_ATTRIBUTES.BACKGROUND_RED | CHAR_ATTRIBUTES.FOREGROUND_BLUE |
                        CHAR_ATTRIBUTES.FOREGROUND_GREEN | CHAR_ATTRIBUTES.FOREGROUND_RED | CHAR_ATTRIBUTES.FOREGROUND_INTENSITY);
                }
            }
        }

        public override string ToString() {
            return "Panel";
        }
    }
}