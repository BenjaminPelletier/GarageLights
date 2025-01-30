using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageLights.Channels
{
    enum ChannelVisibilityState
    {
        ToggleExpandedCollapsed
    }

    interface IChannelElement
    {
        string FullName { get; }
        ChannelNode ChannelNode { get; }
        IEnumerable<IChannelElement> Children { get; }
        Rectangle Bounds { get; }
        bool Visible { get; }
        bool Selected { get; set; }
        bool Expanded { get; set; }

        event EventHandler SelectedChanged;
    }

    class ChannelSelector
    {
        private List<IChannelElement> topElements;

        public event EventHandler SelectedChannelsChanged;

        public void SetTopElements(IEnumerable<IChannelElement> topElements)
        {
            if (topElements != null)
            {
                foreach (IChannelElement element in topElements)
                {
                    UnhookEvents(element);
                }
            }
            this.topElements = topElements.ToList();
            foreach (IChannelElement element in topElements)
            {
                HookEvents(element);
            }
        }

        private void HookEvents(IChannelElement element)
        {
            element.SelectedChanged += IChannelElement_SelectedChanged;
            foreach (IChannelElement child in element.Children)
            {
                HookEvents(child);
            }
        }

        private void UnhookEvents(IChannelElement element)
        {
            element.SelectedChanged -= IChannelElement_SelectedChanged;
            foreach (IChannelElement child in element.Children)
            {
                UnhookEvents(child);
            }
        }

        private void IChannelElement_SelectedChanged(object sender, EventArgs e)
        {
            SelectedChannelsChanged?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerable<IChannelElement> GetChannelElements()
        {
            if (topElements == null) { yield break; }
            foreach (IChannelElement topElement in topElements)
            {
                foreach (IChannelElement element in AllElements(topElement))
                {
                    yield return element;
                }
            }
        }

        private static IEnumerable<IChannelElement> AllElements(IChannelElement parent)
        {
            yield return parent;
            foreach (IChannelElement child in parent.Children)
            {
                foreach (IChannelElement descendant in AllElements(child))
                {
                    yield return descendant;
                }
            }
        }

        public IChannelElement GetChannelElementByName(string fullName)
        {
            if (topElements == null) { return null; }
            foreach (IChannelElement element in topElements)
            {
                if (element.FullName == fullName) { return element; }
            }
            return null;
        }
    }

    static class ChannelSelectorExtensions
    {
        public static IChannelElement ClosestChannel(this ChannelSelector selector, float y)
        {
            IChannelElement closestNode = null;
            float dy = float.PositiveInfinity;
            foreach (IChannelElement element in selector.GetVisibleChannelElements())
            {
                var bounds = element.Bounds;
                if (bounds.Top <= y && y <= bounds.Bottom)
                {
                    return element;
                }
                float cdy = y < bounds.Top ? bounds.Top - y : y - bounds.Bottom;
                if (closestNode == null || cdy < dy)
                {
                    closestNode = element;
                    dy = cdy;
                }
            }
            return closestNode;
        }

        public static IEnumerable<IChannelElement> GetVisibleChannelElements(this ChannelSelector selector)
        {
            return selector.GetChannelElements().Where(n => n.Visible);
        }

        public static IEnumerable<IChannelElement> GetSelectedChannelElements(this ChannelSelector selector)
        {
            return selector.GetChannelElements().Where(n => n.Selected && n.ChannelNode.Channel != null);
        }
    }
}
