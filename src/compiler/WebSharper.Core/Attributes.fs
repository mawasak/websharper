// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2016 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

/// Defines custom attributes used by WebSharper projects.
namespace WebSharper

type private A = System.Attribute
type private T = System.AttributeTargets
type private U = System.AttributeUsageAttribute

/// Marks union cases or properties that should be compiled to constants.
[<Sealed; U(T.Property)>]
type ConstantAttribute private () =
    inherit A()

    /// Constructs a boolean constant annotation.
    new (value: bool) = ConstantAttribute()

    /// Constructs an integer constant annotation.
    new (value: int) = ConstantAttribute()

    /// Constructs a floating constant annotation.
    new (value: float) = ConstantAttribute()

    /// Constructs a string or a null constant annotation.
    new (value: string) = ConstantAttribute()

/// Marks methods and constructors for inline compilation to JavaScript.
/// Inline members work by expanding JavaScript code templates
/// with placeholders of the form such as $0, $x, $this or $value
/// directly at the place of invocation. See also DirectAttribute.
[<Sealed; U(T.Constructor|||T.Method|||T.Property)>]
type InlineAttribute(template: string) =
    inherit A()

    member this.Template = template

    new () = InlineAttribute(null)

/// Marks methods and constructors for direct compilation to a JavaScript function.
/// Direct members work by expanding JavaScript code templates
/// with placeholders of the form such as $0, $x, $this or $value
/// into the body of a JavaScript function. See also InlineAttribute.
[<Sealed; U(T.Constructor|||T.Method|||T.Property)>]
type DirectAttribute(template: string) =
    inherit A()

/// Marks methods and constructors as pure, so the call may be erased by optimizer
/// or applied in different execution order. 
[<Sealed; U(T.Constructor|||T.Method|||T.Property)>]
type PureAttribute() =
    inherit A()

/// Marks methods, properties and constructors for compilation to JavaScript.
[<Sealed; U(T.Assembly|||T.Class|||T.Interface|||T.Module|||T.Constructor|||T.Method|||T.Property|||T.Event|||T.Struct)>]
type JavaScriptAttribute() =
    inherit A()

    new (enabled: bool) = JavaScriptAttribute()

/// Annotates methods an constructors with custom compilation rules.
/// The supplied type should implement Macros.IMacro and a default constructor.
[<Sealed; U(T.Class|||T.Interface|||T.Constructor|||T.Method|||T.Property|||T.Event|||T.Struct, AllowMultiple = true)>]
type MacroAttribute private () =
    inherit A()

    new (macroType: System.Type) = MacroAttribute()
    new (macroType: System.Type, parameter: obj) = MacroAttribute()
    new (assemblyQualifiedName: string) = MacroAttribute() 
    new (assemblyQualifiedName: string, parameter: obj) = MacroAttribute() 

/// Annotates methods with a generator type that provides the method body.
/// The supplied type should implement Macros.IGenerator and a default constructor.
[<Sealed; U(T.Constructor|||T.Method|||T.Property)>]
type GeneratedAttribute private () =
    inherit A()

    new (generatorType: System.Type) = GeneratedAttribute()
    new (generatorType: System.Type, parameter: obj) = GeneratedAttribute()
    new (assemblyQualifiedName: string) = GeneratedAttribute() 
    new (assemblyQualifiedName: string, parameter: obj) = GeneratedAttribute() 

/// Provides a runtime name for members when it differs from the F# name.
/// The constructor accepts either an explicit array of parts,
/// or a single string, in which case it is assumed to be dot-separated.
[<Sealed; U(T.Class|||T.Interface|||T.Constructor|||T.Method|||T.Property|||T.Field|||T.Event|||T.Struct)>]
type NameAttribute private () =
    inherit A()

    /// Constructs a qualified name from a dot-separated string.
    new (name: string) = NameAttribute()

    /// Constructs an indexed field.
    new (index: int) = NameAttribute()

    /// Constructs a qualified name from an explicit array of parts.
    new ([<System.ParamArray>] names: string []) = NameAttribute()

/// Declares a type to be a proxy for another type, identified directly or
/// by using an assembly-qualified name.
[<Sealed; U(T.Class|||T.Interface|||T.Struct)>]
type ProxyAttribute private () =
    inherit A()

    /// Constructs a new proxy link using a type directly.
    new (proxiedType: System.Type) = ProxyAttribute()

    /// Constructs a new proxy link using an assembly-qualified name.
    new (assemblyQualifiedName: string) = ProxyAttribute()

/// Marks a server-side function to be invokable remotely from the client-side.
[<Sealed; U(T.Method)>]
type RemoteAttribute() =
    inherit A()

/// Annotates members with dependencies. The type passed to the constructor
/// must implement Resources.IResourceDefinition and a default constructor.
[<Sealed; U(T.Assembly|||T.Class|||T.Constructor|||T.Method|||T.Property|||T.Event|||T.Struct,
            AllowMultiple=true)>]
type RequireAttribute private () =

    new (resourceType: System.Type) = RequireAttribute()

    new (assemblyQualifiedName: string) = RequireAttribute()

/// Marks members that should be compiled by-name.
[<Sealed; U(T.Class|||T.Constructor|||T.Method|||T.Property|||T.Struct)>]
type StubAttribute() =
    inherit A()

/// Indicates the client-side remoting provider that should be used
/// by remote function calls in this assembly. The type passed to the
/// constructor must have three static methods as described by the
/// interface Remoting.IRemotingProvider.
[<Sealed; U(T.Assembly|||T.Class|||T.Method|||T.Struct)>]
type RemotingProviderAttribute private () =
    inherit A()

    new (remotingProviderType: System.Type) = RemotingProviderAttribute()

    new (assemblyQualifiedName: string) = RemotingProviderAttribute()

/// Adds automatic inlines to a property so that a missing JavaScript field
/// is converted to None, otherwise Some fieldValue.
[<Sealed; U(T.Class|||T.Property|||T.Field|||T.Struct)>]
type OptionalFieldAttribute() =
    inherit A()

/// Declares that when de/serializing this union type for external use
/// (eg. when parsing a [<Json>] sitelet action or writing a Sitelet.Content.JsonContent),
/// its fields must be tagged by their name rather than "$0" ... "$n".
/// Also determines how the cases are distinguished, instead of the default "$": <integer>.
[<Sealed; U(T.Class)>]
type NamedUnionCasesAttribute =
    inherit A

    /// The case is determined by a field named `discriminatorName`,
    /// which stores the CompiledName of the case.
    new (discriminatorName: string) = { inherit A() }

    /// The case is inferred from the field names. Every case must have at least one
    /// non-option-typed field whose name is unique across all cases of this union.
    new () = { inherit A() }

/// Defines the format used to de/serialize a DateTime field or union case argument.
/// The default is "o" (ISO 8601 round-trip format) for JSON serialization,
/// and "yyyy-MM-dd-HH.mm.ss" for URL parsing.
[<Sealed; U(T.Property, AllowMultiple = true)>]
type DateTimeFormatAttribute =
    inherit A

    /// Defines the format used to de/serialize a record or object field.
    new (format: string) = { inherit A() }

    /// Defines the format used to de/serialize the union case argument with the given name.
    new (argumentName: string, format: string) = { inherit A() }

/// Marks the entry point of a Single Page Application.
[<Sealed; U(T.Method)>]
type SPAEntryPointAttribute() = 
    inherit A()