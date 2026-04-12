//using CommunityToolkit.Maui.Behaviors;
//using MauiLearn.Core.Services.Interfaces;
//using MauiLearn.Core.Services.Messages;
//using Microsoft.Maui.ApplicationModel;
//using Microsoft.VisualBasic;
//using System.Reflection;
//using System.Text;

///*********************************************************************************
// *                         VISUAL ELEMENTS 
// * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//     Summary:
//        that occupies an area on the screen, has a visual appearance, 
//        and can obtain touch input.
//**********************************************************************************/
//namespace MauiLearn.MultiApp.Converters //Change this later to out project Core.Services
//{
//    /// <summary> 
//    /// The <see cref="LoginEventToCommandBehavior"/> is a behavior that allows the user to invoke a 
//    /// <see cref = "ICommand" /> through an event. It is designed to associate Commands to events
//    /// exposed by controls that were not designed to support Commands. It allows yo u to map any
//    /// arbitrary event on a control to a Command. 
//    /// </summary> 
//    public class LoginEventToCommandBehavior : BaseBehavior<VisualElement>
//    {
//        BindableProperty bindableProperty;

//        //3/25/2026 - just taking from Enterprise-Applications book...learning more with this
//        // all with objects UI controls... 
//        string m_elementName;
//        IList<Behavior>? m_elementBehaviorsList;

//        EventInfo? eventInfo = null;
//        Delegate? eventHandler = null;

//        //represents most on-screen UI elements.It provides the core properties, events,
//        //and methods needed for rendering and interacting with visual components in a
//        //cross-platform application
//        protected override void OnAttachedTo(VisualElement bindable)
//        {
//            base.OnAttachedTo(bindable);
//            RegisterEvent();
//            m_elementBehaviorsList = bindable.Behaviors;//.GetType().Name;
//        }

//        /// <inheritdoc/> 
//        protected override void OnDetachingFrom(VisualElement bindable)
//        {
//            UnregisterEvent();
//            base.OnDetachingFrom(bindable);

//        }

//        static void OnEventNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
//            => ((EventToCommandBehavior)bindable).RegisterEvent();

  

//        void RegisterEvent()
//        {
//            UnregisterEvent();

//            if (View is null || string.IsNullOrWhiteSpace(m_elementName))
//            {
//                return;
//            }

//            eventInfo = View.GetType()?.GetRuntimeEvent(m_elementName) ??
//                throw new ArgumentException($"{nameof(EventToCommandBehavior)}: Couldn't resolve the event.", nameof(m_elementName));

//            ArgumentNullException.ThrowIfNull(eventInfo.EventHandlerType);
//            var eventHandlerMethodInfo = eventInfo.GetAddMethod(); //3/25/2026
//            ArgumentNullException.ThrowIfNull(eventHandlerMethodInfo);//3/25/2026
//            //3/25/2026 - all fields entired up and after the omitted for brevity....
//            eventHandler = eventHandlerMethodInfo.CreateDelegate(eventInfo.EventHandlerType, this) ??
//                throw new ArgumentException($"{nameof(EventToCommandBehavior)}: Couldn't create event handler.", nameof(m_elementName));

//            eventInfo.AddEventHandler(View, eventHandler);
//        }

//        void UnregisterEvent()
//        {
//            if (eventInfo is not null && eventHandler is not null)
//            {
//                eventInfo.RemoveEventHandler(View, eventHandler);
//            }

//            eventInfo = null;
//            eventHandler = null;
//        }

//        /// <summary> 
//        /// Virtual method that executes when a Command is invoked 
//        /// </summary> 
//        /// <param name="sender"></param> 
//        /// <param name="eventArgs"></param> 
//        [Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
//        protected virtual void OnTriggerHandled(object? sender = null, object? eventArgs = null)
//        {
//            //3/25/206 - the of ? is/has already begin with the command
//            //list behaviors
//            foreach (var prop in m_elementBehaviorsList)
//            {
//                if (prop != null)
//                {

//                    //prop.PropertyChanging -= OnPropertyChanging;    
//                }
//            }

//            ////want the parameters that may being brought in
//            //var parameter = CommandParameter
//            //     ?? EventArgsConverter?.Convert(eventArgs, typeof(object), null, null);

//            //List<Delegate>? list = new List<Delegate>();
//            ////want the parameters that may being brought in
//            //if (!eventHandler.TryGetValue(type, out var list))
//            //    return;

//            ////after knowing the parameters 
//            //eventHandler.DynamicInvoke(parameter, sender, eventArgs);


//            //var command = Command;
//            //if (command?.CanExecute(parameter) ?? false)
//            //{
//            //    command.Execute(parameter);
//            //}
//        }
//    }
//}
