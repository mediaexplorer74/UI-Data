namespace Get.UI.Data;
public delegate TTemplateParts ExternalControlTemplate<TTemplateParts, TIn>(TIn rootElement) where
    TTemplateParts : struct;
public delegate TTemplateParts ExternalControlTemplate<TTemplateParts, TControlType, TIn>(TControlType parent, TIn rootElement) where
    TTemplateParts : struct;