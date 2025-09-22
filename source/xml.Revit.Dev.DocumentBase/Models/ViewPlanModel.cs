/* 作    者: xml
** 创建时间: 2024/6/11 18:05:01
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

using xml.Revit.Dev.DocumentBase.Extensions;

namespace xml.Revit.Dev.DocumentBase.Models
{
    /// <summary>
    /// 楼层平面
    /// </summary>
    public sealed class ViewPlanModel : ElementModel
    {
        public ViewPlanModel() { }

        public ViewPlanModel(ViewPlan viewPlan)
        {
            UniqueId = viewPlan.UniqueId;
            Name = viewPlan.Name;
            ElementTypeName = GetElementTypeName(viewPlan);

            Scale = viewPlan.Scale;
            LevelName = viewPlan.GenLevel.Name;
            CropBoxActive = viewPlan.CropBoxActive;
            CropBoxVisible = viewPlan.CropBoxVisible;

            AnnotationAcvice = viewPlan
                .get_Parameter(BuiltInParameter.VIEWER_ANNOTATION_CROP_ACTIVE)
                .AsInteger();

            var boundingBox = viewPlan.CropBox;
            if (boundingBox != null)
            {
                CropBoxMin = boundingBox.Min.ToPointModel();
                CropBoxMax = boundingBox.Max.ToPointModel();
            }

            DetailLevel = viewPlan.DetailLevel;
            Discipline = viewPlan.Discipline;
            DisplayStyle = viewPlan.DisplayStyle;
            PartsVisibility = viewPlan.PartsVisibility;

            var doc = viewPlan.Document;
            if (viewPlan.ViewTemplateId != ElementId.InvalidElementId)
            {
                ViewTemplateName = viewPlan.ViewTemplateId.ToElement(doc).Name;
            }

            ViewFamily = (viewPlan.GetTypeId().ToElement(doc) as ViewFamilyType).ViewFamily;
        }

        public int Scale { get; set; }
        public string LevelName { get; set; }

        public bool CropBoxActive { get; set; }
        public bool CropBoxVisible { get; set; }

        /// <summary>
        /// 注释裁剪
        /// VIEWER_ANNOTATION_CROP_ACTIVE
        /// </summary>
        public int AnnotationAcvice { get; set; }
        public PointModel CropBoxMin { get; set; }
        public PointModel CropBoxMax { get; set; }

        public ViewDetailLevel DetailLevel { get; set; }
        public ViewDiscipline Discipline { get; set; }
        public DisplayStyle DisplayStyle { get; set; }
        public PartsVisibility PartsVisibility { get; set; }

        public string ViewTemplateName { get; set; } = string.Empty;
        public ViewFamily ViewFamily { get; set; }
    }
}
