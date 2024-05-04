module Thoth.Json.Tests.Testing

open Thoth.Json.Core

// #if FABLE_COMPILER
// open Fable.Core.Testing
// #else
// open Expecto
// #endif

// // let testList (name : string) (tests: seq<'b>) = name, tests
// // let testCase (msg : string) (test : obj -> unit) = msg, test

// let inline equal expected actual: unit =
//     #if FABLE_COMPILER
//         Assert.AreEqual(actual, expected)
//     #else
//         Expect.equal actual expected ""
//     #endif

// // type Test = string * seq<string * seq<string * (obj -> unit)>>


// type ITestContrat =
//     abstract

type IEncode =
    abstract toString: int -> IEncodable -> string

type IDecode =
    abstract fromString<'T> : Decoder<'T> -> string -> Result<'T, string>
    abstract unsafeFromString<'T> : Decoder<'T> -> string -> 'T

[<AbstractClass>]
type TestRunner<'Test, 'JsonValue>() =
    abstract testList: (string -> 'Test list -> 'Test) with get
    abstract testCase: (string -> (unit -> unit) -> 'Test) with get
    abstract ftestCase: (string -> (unit -> unit) -> 'Test) with get
    abstract equal<'a when 'a: equality> : actual: 'a -> expected: 'a -> unit
    abstract Encode: IEncode with get
    abstract Decode: IDecode with get
