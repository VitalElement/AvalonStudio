namespace AvalonStudio.Extensibility.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    //using Caliburn.Micro;
    using Perspex.Controls;
    using Perspex;
    using Perspex.VisualTree;

    [Export(typeof(ICommandRouter))]
    public class CommandRouter : ICommandRouter
    {
        private static readonly Type CommandHandlerInterfaceType = typeof(ICommandHandler<>);
        private static readonly Type CommandListHandlerInterfaceType = typeof(ICommandListHandler<>);

        private readonly Dictionary<Type, CommandHandlerWrapper> _globalCommandHandlerWrappers;
        private readonly Dictionary<Type, HashSet<Type>> _commandHandlerTypeToCommandDefinitionTypesLookup;
            
        [ImportingConstructor]
        public CommandRouter(
            [ImportMany(typeof(ICommandHandler))] ICommandHandler[] globalCommandHandlers)
        {
            _commandHandlerTypeToCommandDefinitionTypesLookup = new Dictionary<Type, HashSet<Type>>();
            _globalCommandHandlerWrappers = BuildCommandHandlerWrappers(globalCommandHandlers);
        }

        private Dictionary<Type, CommandHandlerWrapper> BuildCommandHandlerWrappers(ICommandHandler[] commandHandlers)
        {
            var commandHandlersList = SortCommandHandlers(commandHandlers);

            // Command handlers are either ICommandHandler<T> or ICommandListHandler<T>.
            // We need to extract T, and use it as the key in our dictionary.

            var result = new Dictionary<Type, CommandHandlerWrapper>();

            foreach (var commandHandler in commandHandlersList)
            {
                var commandHandlerType = commandHandler.GetType();
                EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(commandHandlerType);
                var commandDefinitionTypes = _commandHandlerTypeToCommandDefinitionTypesLookup[commandHandlerType];
                foreach (var commandDefinitionType in commandDefinitionTypes)
                    result[commandDefinitionType] = CreateCommandHandlerWrapper(commandDefinitionType, commandHandler);
            }

            return result;
        }

        private static List<ICommandHandler> SortCommandHandlers(ICommandHandler[] commandHandlers)
        {
            // Put command handlers defined in priority assemblies, last. This allows applications
            // to override built-in command handlers.

            throw new NotImplementedException();
            //var bootstrapper = IoC.Get<AppBootstrapper>();

            //return commandHandlers
            //    .OrderBy(h => bootstrapper.PriorityAssemblies.Contains(h.GetType().Assembly) ? 1 : 0)
            //    .ToList();
        }

        public CommandHandlerWrapper GetCommandHandler(CommandDefinitionBase commandDefinition)
        {
            CommandHandlerWrapper commandHandler;

            throw new NotImplementedException();
            //var shell = IoC.Get<IShell>();

            //var activeItemViewModel = shell.ActiveLayoutItem;
            //if (activeItemViewModel != null)
            //{
            //    commandHandler = GetCommandHandlerForLayoutItem(commandDefinition, activeItemViewModel);
            //    if (commandHandler != null)
            //        return commandHandler;
            //}

            //var activeDocumentViewModel = shell.ActiveItem;
            //if (activeDocumentViewModel != null && !Equals(activeDocumentViewModel, activeItemViewModel))
            //{
            //    commandHandler = GetCommandHandlerForLayoutItem(commandDefinition, activeDocumentViewModel);
            //    if (commandHandler != null)
            //        return commandHandler;
            //}

            //// If none of the objects in the DataContext hierarchy handle the command,
            //// fallback to the global handler.
            //if (!_globalCommandHandlerWrappers.TryGetValue(commandDefinition.GetType(), out commandHandler))
            //    return null;

            //return commandHandler;
        }

        private CommandHandlerWrapper GetCommandHandlerForLayoutItem(CommandDefinitionBase commandDefinition, object activeItemViewModel)
        {
            throw new NotImplementedException();
            //var activeItemView = ViewLocator.LocateForModel(activeItemViewModel, null, null);
            //var activeItemWindow = Window.GetWindow(activeItemView);
            //if (activeItemWindow == null)
            //    return null;

            //var startElement = FocusManager.GetFocusedElement(activeItemView) ?? activeItemView;

            //// First, we look at the currently focused element, and iterate up through
            //// the tree, giving each DataContext a chance to handle the command.
            //return FindCommandHandlerInVisualTree(commandDefinition, startElement);
        }

        private CommandHandlerWrapper FindCommandHandlerInVisualTree(CommandDefinitionBase commandDefinition, Control target)
        {
            var visualObject = target as PerspexObject;
            if (visualObject == null)
                return null;

            object previousDataContext = null;
            do
            {
                var frameworkElement = visualObject as Control;
                if (frameworkElement != null)
                {
                    var dataContext = frameworkElement.DataContext;
                    if (dataContext != null && !ReferenceEquals(dataContext, previousDataContext))
                    {
                        if (dataContext is ICommandRerouter)
                        {
                            var commandRerouter = (ICommandRerouter) dataContext;
                            var commandTarget = commandRerouter.GetHandler(commandDefinition);
                            if (commandTarget != null)
                            {
                                if (IsCommandHandlerForCommandDefinitionType(commandTarget, commandDefinition.GetType()))
                                    return CreateCommandHandlerWrapper(commandDefinition.GetType(), commandTarget);
                                throw new InvalidOperationException("This object does not handle the specified command definition.");
                            }
                        }

                        if (IsCommandHandlerForCommandDefinitionType(dataContext, commandDefinition.GetType()))
                            return CreateCommandHandlerWrapper(commandDefinition.GetType(), dataContext);

                        previousDataContext = dataContext;
                    }
                }

                throw new NotImplementedException();
                //visualObject = VisualTreeHelper.GetParent(visualObject);
            } while (visualObject != null);

            return null;
        }

        private static CommandHandlerWrapper CreateCommandHandlerWrapper(
            Type commandDefinitionType, object commandHandler)
        {
            if (typeof(CommandDefinition).IsAssignableFrom(commandDefinitionType))
                return CommandHandlerWrapper.FromCommandHandler(CommandHandlerInterfaceType.MakeGenericType(commandDefinitionType), commandHandler);
            if (typeof(CommandListDefinition).IsAssignableFrom(commandDefinitionType))
                return CommandHandlerWrapper.FromCommandListHandler(CommandListHandlerInterfaceType.MakeGenericType(commandDefinitionType), commandHandler);
            throw new InvalidOperationException();
        }

        private bool IsCommandHandlerForCommandDefinitionType(
            object commandHandler, Type commandDefinitionType)
        {
            var commandHandlerType = commandHandler.GetType();
            EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(commandHandlerType);
            var commandDefinitionTypes = _commandHandlerTypeToCommandDefinitionTypesLookup[commandHandlerType];
            return commandDefinitionTypes.Contains(commandDefinitionType);
        }

        private void EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(Type commandHandlerType)
        {
            if (!_commandHandlerTypeToCommandDefinitionTypesLookup.ContainsKey(commandHandlerType))
            {
                var commandDefinitionTypes = _commandHandlerTypeToCommandDefinitionTypesLookup[commandHandlerType] = new HashSet<Type>();

                foreach (var handledCommandDefinitionType in GetAllHandledCommandedDefinitionTypes(commandHandlerType, CommandHandlerInterfaceType))
                    commandDefinitionTypes.Add(handledCommandDefinitionType);

                foreach (var handledCommandDefinitionType in GetAllHandledCommandedDefinitionTypes(commandHandlerType, CommandListHandlerInterfaceType))
                    commandDefinitionTypes.Add(handledCommandDefinitionType);
            }
        }

        private static IEnumerable<Type> GetAllHandledCommandedDefinitionTypes(
            Type type, Type genericInterfaceType)
        {
            var result = new List<Type>();

            while (type != null)
            {
                result.AddRange(type.GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType)
                    .Select(x => x.GetGenericArguments().First()));

                type = type.BaseType;
            }

            return result;
        }
    }
}