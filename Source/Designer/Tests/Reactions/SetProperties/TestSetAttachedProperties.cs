﻿using NUnit.Framework;
using Xamarin.Forms;
using XenForms.Core.Designer.Reactions;
using XenForms.Core.Messages;
using XenForms.Core.Networking;
using XenForms.Designer.XamarinForms.UI.Reactions;

namespace XenForms.Designer.Tests.Reactions.SetProperties
{
    [TestFixture]
    public class TestSetAttachedProperties : TestBaseReaction
    {
        [Test]
        public void Should_set_grid_attached_properties_on_direct_child()
        {
            var grid = new Grid();
            var label = new Label();
            grid.Children.Add(label);
            var id = label.Id.ToString();

            var page = new ContentPage
            {
                Content = grid
            };

            XamarinFormsReaction.Register<SetPropertyRequest, SetPropertyReaction>(page);

            // row
            var rCtx = new XenMessageContext();
            rCtx.SetRequest<SetPropertyRequest>(r =>
            {
                r.Path = new[] { "RowProperty" };
                r.WidgetId = id;
                r.Value = 2;
                r.IsAttachedProperty = true;
                r.IsBase64 = false;
            });

            Reaction.Execute(rCtx);
            Assert.AreEqual(2, Grid.GetRow(label));

            // column
            var cCtx = new XenMessageContext();
            cCtx.SetRequest<SetPropertyRequest>(r =>
            {
                r.Path = new[] { "ColumnProperty" };
                r.WidgetId = id;
                r.Value = 3;
                r.IsAttachedProperty = true;
                r.IsBase64 = false;
            });

            Reaction.Execute(cCtx);
            Assert.AreEqual(3, Grid.GetColumn(label));
        }


        [Test]
        public void Should_set_attached_properties_on_descendants_child()
        {
            var first = new Grid();
            var second = new Grid();
            first.Children.Add(second);
            Grid.SetRow(first, 1);
            Grid.SetRow(second, 2);

            Grid.SetColumn(first, 1);
            Grid.SetColumn(second, 2);

            var label = new Label();
            var id = label.Id.ToString();
            second.Children.Add(label);

            Grid.SetRow(label, 3);
            Grid.SetColumn(label, 3);

            var page = new ContentPage
            {
                Content = first
            };

            var ctx = new XenMessageContext();
            ctx.SetRequest<SetPropertyRequest>(r =>
            {
                r.Path = new[] { "ColumnProperty" };
                r.WidgetId = id;
                r.Value = 99;
                r.IsAttachedProperty = true;
                r.IsBase64 = false;
            });

            XamarinFormsReaction.Register<SetPropertyRequest, SetPropertyReaction>(page);
            Reaction.Execute(ctx);

            Assert.AreEqual(99, Grid.GetColumn(label));
        }
    }
}
