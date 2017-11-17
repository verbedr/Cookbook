using Common.Services;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Cookbook.Castle
{
    public static class ServiceBuilder
    {
        static Type mediatorType = typeof(IRequestHandlerMediator);
        static MethodInfo exectueGeneric = mediatorType.GetMethod("ExecuteAsync");
        static AssemblyBuilder builder = AssemblyBuilder
                .DefineDynamicAssembly(new AssemblyName(typeof(ServiceBuilder).FullName), AssemblyBuilderAccess.Run);
        static ModuleBuilder module = builder.DefineDynamicModule("MainModule");

        public static Type CompileResultType(Type iservice)
        {
            var type = CreateType(iservice);
            var mediator = type.DefineField("_mediator", mediatorType, FieldAttributes.InitOnly);
            CreateConstructor(type, mediator);

            foreach (var method in iservice.GetMethods())
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 1)
                    throw new Exception("Service interface incompatible with convention. We can only handle ReturnType MethodName(ParameterType request)");

                var implementation = CreateImplementation(mediator, type, method.Name, method.ReturnType, parameters[0]);
                type.DefineMethodOverride(implementation, method);
            }
            return type.CreateType();
        }

        private static TypeBuilder CreateType(Type iservice)
        {
            var typeSignature = iservice.Namespace + "." + iservice.Name.Remove(0, 1);
            var returnValue = module.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null, new[] { iservice });
            return returnValue;
        }

        private static void CreateConstructor(TypeBuilder type, FieldBuilder mediator)
        {
            var constructor = type
                .DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { mediatorType });
            var constructorBody = constructor.GetILGenerator();
            constructorBody.Emit(OpCodes.Ldarg_0);
            constructorBody.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));
            constructorBody.Emit(OpCodes.Ldarg_0);
            constructorBody.Emit(OpCodes.Ldarg_1);
            constructorBody.Emit(OpCodes.Stfld, mediator);
            constructorBody.Emit(OpCodes.Ret);
        }

        private static MethodBuilder CreateImplementation(FieldBuilder mediator, TypeBuilder type, string name, Type returnType, ParameterInfo parameterInfo)
        {
            var method = type.DefineMethod(name, MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
                returnType, new[] { parameterInfo.ParameterType });
            var methodBody = method.GetILGenerator();
            methodBody.Emit(OpCodes.Ldarg_0);
            methodBody.Emit(OpCodes.Ldfld, mediator);
            methodBody.Emit(OpCodes.Ldarg_1);
            var execute = exectueGeneric.MakeGenericMethod(new[] 
            {
                parameterInfo.ParameterType,
                returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)
                    ? returnType.GenericTypeArguments[0]
                    : returnType});
            methodBody.Emit(OpCodes.Callvirt, execute);
            methodBody.Emit(OpCodes.Ret);
            return method;
        }
    }
}
