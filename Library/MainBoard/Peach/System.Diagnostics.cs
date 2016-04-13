namespace System.Diagnostics
{
#pragma warning disable 1591
    // .NET Micro Framework 4.3 QFE2 ではこれの定義が見つからず実行時エラーになる
    // これを回避するために、System.Diagnostics.DebuggerBrowsableState 列挙体を定義しておく
    public enum DebuggerBrowsableState
    {
        Never,
        Collapsed,
        RootHidden
    }
#pragma warning restore 1591
}