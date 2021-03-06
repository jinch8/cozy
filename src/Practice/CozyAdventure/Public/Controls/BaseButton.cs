﻿using CocosSharp;
using CozyAdventure.Game.Manager;
using CozyAdventure.Public.Controls.Enum;
using System;

namespace CozyAdventure.Public.Controls
{
    public abstract class BaseButton : CozyControl
    {
        #region Text

        private CCLabel DisplayText { get; set; }

        private string text;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value != null && value != text && value.Length > 0)
                {
                    text = value;
                    RefreshText();
                }
            }
        }

        private int fontSize = 12;

        public int FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
                RefreshText();
            }
        }

        private void RefreshText()
        {
            if (DisplayText != null)
            {
                this.RemoveChild(DisplayText);
            }
            DisplayText = new CCLabel(text, StringManager.GetText("GlobalFont"), FontSize);
            DisplayText.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
            this.AddChild(DisplayText, 2);
        }

        #region 缩放用

        private CCScaleTo m_scaleTo;
        private CCScaleTo m_scaleForm;
        private CCCallFuncN m_scaleFunc;

        private float scaleTo;
        private float scaleFrom;
        private float scaleDuration;

        public float ScaleTo
        {
            get
            {
                return scaleTo;
            }
            set
            {
                if (Math.Abs(value - scaleTo) > float.Epsilon)
                {
                    scaleTo = value;

                    m_scaleForm = new CCScaleTo(ScaleDuration, scaleTo);
                }
            }
        }

        public float ScaleFrom
        {
            get
            {
                return scaleFrom;
            }
            set
            {
                if (Math.Abs(value - scaleFrom) > float.Epsilon)
                {
                    scaleFrom = value;
                    m_scaleForm = new CCScaleTo(ScaleDuration, scaleFrom);
                }
            }
        }

        public float ScaleDuration
        {
            get
            {
                return scaleDuration;
            }
            set
            {
                if (Math.Abs(value - scaleDuration) > float.Epsilon)
                {
                    scaleDuration = value;

                    m_scaleTo = new CCScaleTo(scaleDuration, ScaleTo);
                    m_scaleForm = new CCScaleTo(scaleDuration, ScaleFrom);
                }
            }
        }

        #endregion 缩放用

        #endregion Text

        #region Constructors

        /// <summary>
        /// 初始化缩放
        /// </summary>
        /// <param name="_scaleForm"></param>
        /// <param name="_scaleTo"></param>
        /// <param name="_duration"></param>
        private void InitScale(float _scaleForm = 1.0f, float _scaleTo = 1.1f, float _duration = 0.1f)
        {
            m_scaleFunc = new CCCallFuncN(node => ((BaseButton)node).ScaleComplete());

            ScaleTo = _scaleTo;
            ScaleFrom = _scaleForm;
            ScaleDuration = _duration;
        }

        public BaseButton(float width, float height)
        {
            InitScale();

            Init(width, height);
        }

        public BaseButton(float x, float y, float width, float height)
        {
            InitScale();

            InitWithRect(x, y, width, height);
        }

        public virtual void Init(float width, float height)
        {
            ContentSize = new CCSize(width, height);
        }

        public virtual void InitWithRect(float x, float y, float width, float height)
        {
            Init(width, height);

            Position = new CCPoint(x, y);
        }

        #endregion Constructors

        #region Event

        private CCPoint StartPoint { get; set; }

        public Action OnClick { get; set; }

        public ButtonStatus Status { get; set; }

        public bool OnTouchBegan(CCTouch touch, CCEvent e)
        {
            StartPoint = touch.Location;
            OnKeyDown();
            return true;
        }

        public void OnTouchEnded(CCTouch touch, CCEvent e)
        {
            if (!Visible)
            {
                return;
            }

            var beginrect = new CCRect(StartPoint.X, StartPoint.Y, ContentSize.Width, ContentSize.Height);
            if(!beginrect.ContainsPoint(touch.Location))
            {
                return;
            }

            var rect = new CCRect(PositionWorldspace.X, PositionWorldspace.Y, ContentSize.Width, ContentSize.Height);
            if (!rect.ContainsPoint(touch.Location))
            {
                return;
            }
            OnKeyUp();
        }

        protected virtual void OnKeyDown()
        {
            Status = ButtonStatus.Pressed;
        }

        protected virtual void OnKeyUp()
        {
            // 缩放
            this.RunActions(m_scaleTo, m_scaleFunc);

            Status = ButtonStatus.Released;
        }

        /// <summary>
        /// 缩放结束后弹回
        /// </summary>
        private void ScaleComplete()
        {
            this.RunAction(m_scaleForm);

            // 弹起后响应这样更符合操作.
            OnClick();
        }

        #endregion Event
    }
}