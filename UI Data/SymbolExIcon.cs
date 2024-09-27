using System;
using System.Collections.Generic;
using System.Text;

namespace Get.Symbols;

public class SymbolExIcon : FontIcon
{
    public SymbolExIcon()
    {
        FontFamily = (FontFamily)Resources["SymbolThemeFontFamily"];
    }
    public SymbolExIcon(SymbolEx SymbolEx) : this()
    {
        this.SymbolEx = SymbolEx;
    }
    public SymbolExIcon(Symbol Symbol) : this()
    {
        this.Symbol = Symbol;
    }
    public SymbolEx SymbolEx
    {
        get => (SymbolEx)Glyph[0];
        set => Glyph = ((char)value).ToString();
    }
    public Symbol Symbol
    {
        get => (Symbol)Glyph[0];
        set => Glyph = ((char)value).ToString();
    }
}
