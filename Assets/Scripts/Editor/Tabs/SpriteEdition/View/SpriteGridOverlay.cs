using UnityEngine;
using UnityEngine.UIElements;

namespace AnimatorFactory.SpriteEdition
{
    public class SpriteGridOverlay
    {
        VisualElement _gridOverlay;
        readonly Image _targetImage;

        public SpriteGridOverlay(Image targetImage)
        {
            _targetImage = targetImage;
        }

        public void ShowGrid(int rows, int columns, int frameWidth, int frameHeight)
        {
            ClearGrid();
            
            if (_targetImage.sprite == null)
                return;
                
            // Get the actual displayed image dimensions (image is scaled 2x)
            float displayedWidth = _targetImage.sprite.texture.width * 2;
            float displayedHeight = _targetImage.sprite.texture.height * 2;
            
            // Calculate frame dimensions in display coordinates
            float displayFrameWidth = displayedWidth / columns;
            float displayFrameHeight = displayedHeight / rows;
                
            _gridOverlay = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0,
                    left = 0,
                    width = displayedWidth,
                    height = displayedHeight
                }
            };
            
            CreateVerticalGridLines(columns: columns, displayFrameWidth: displayFrameWidth);
            CreateHorizontalGridLines(rows: rows, displayFrameHeight: displayFrameHeight);
            
            // Add overlay as a child of the image itself
            _targetImage.Add(child: _gridOverlay);
        }
        
        public void ClearGrid()
        {
            if (_gridOverlay != null)
            {
                _gridOverlay.RemoveFromHierarchy();
                _gridOverlay = null;
            }
        }

        void CreateVerticalGridLines(int columns, float displayFrameWidth)
        {
            // Create vertical grid lines (including start and end borders)
            for (int col = 0; col <= columns; col++)
            {
                VisualElement verticalLine = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = col * displayFrameWidth,
                        top = 0,
                        width = 1,
                        height = Length.Percent(value: 100),
                        backgroundColor = Color.white
                    }
                };
                _gridOverlay.Add(child: verticalLine);
            }
        }

        void CreateHorizontalGridLines(int rows, float displayFrameHeight)
        {
            // Create horizontal grid lines
            for (int row = 1; row < rows; row++)
            {
                VisualElement horizontalLine = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = 0,
                        top = row * displayFrameHeight,
                        width = Length.Percent(value: 100),
                        height = 1,
                        backgroundColor = Color.white
                    }
                };
                _gridOverlay.Add(child: horizontalLine);
            }
        }
    }
}