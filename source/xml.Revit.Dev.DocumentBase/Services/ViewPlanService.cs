/* 作    者: xml
** 创建时间: 2024/6/11 18:06:01
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

using System.Diagnostics;
using xml.Revit.Dev.DocumentBase.Extensions;
using xml.Revit.Dev.DocumentBase.Models;

namespace xml.Revit.Dev.DocumentBase.Services
{
    public interface IViewPlanService : IXmlElement<ViewPlan, ViewPlanModel> { }

    internal class ViewPlanService : IViewPlanService
    {
        public List<ViewPlan> Create(IEnumerable<ViewPlanModel> models)
        {
            var doc = XmlDoc.Doc;
            var levels = doc.OfClass<Level>().ToList();
            var viewFamilyTypes = doc.OfClass<ViewFamilyType>()
                .Where(o => o.ViewFamily == ViewFamily.FloorPlan);
            var viewPlans = doc.OfClass<ViewPlan>().Where(o => !o.IsTemplate).ToList();
            var viewTemplates = doc.OfClass<ViewPlan>().Where(o => o.IsTemplate).ToList();

            List<ViewPlan> result = [];

            foreach (var model in models)
            {
                var viewPlan = viewPlans.FirstOrDefault(o => o.Name == model.Name);
                if (viewPlan == null)
                {
                    var level = levels.FirstOrDefault(o => o.Name == model.LevelName);
                    if (level == null)
                    {
                        Debug.WriteLine("视图标高获取失败:" + model.LevelName);
                        continue;
                    }

                    var viewFamilyType =
                        viewFamilyTypes.FirstOrDefault(o =>
                            o.ViewFamily == model.ViewFamily && o.Name == model.ElementTypeName
                        ) ?? viewFamilyTypes.FirstOrDefault();

                    viewPlan = ViewPlan.Create(doc, viewFamilyType.Id, level.Id);
                }

                if (viewPlan != null)
                {
                    viewPlan.Scale = model.Scale;
                    viewPlan.CropBoxActive = model.CropBoxActive;
                    viewPlan.CropBoxVisible = model.CropBoxVisible;
                    viewPlan
                        .get_Parameter(BuiltInParameter.VIEWER_ANNOTATION_CROP_ACTIVE)
                        .Set(model.AnnotationAcvice);

                    viewPlan.DetailLevel = model.DetailLevel;
                    viewPlan.Discipline = model.Discipline;
                    viewPlan.DisplayStyle = model.DisplayStyle;
                    viewPlan.PartsVisibility = model.PartsVisibility;

                    var min = model.CropBoxMin.ToXYZ();
                    var max = model.CropBoxMax.ToXYZ();
                    var boundingBox = new BoundingBoxXYZ() { Min = min, Max = max };
                    viewPlan.CropBox = boundingBox;

                    if (!model.ViewTemplateName.IsNullOrEmpty())
                    {
                        var viewTemplate = viewTemplates.FirstOrDefault(o =>
                            o.Name == model.ViewTemplateName
                        );
                        if (viewTemplate != null && viewTemplate.IsValidObject)
                        {
                            viewPlan.ViewTemplateId = viewTemplate.Id;
                        }
                    }

                    result.Add(viewPlan);
                }
            }

            return result;
        }

        public List<ViewPlanModel> GetElementModels()
        {
            var doc = XmlDoc.Doc;
            var viewPlans = doc.OfClass<ViewPlan>().Where(o => !o.IsTemplate).ToList();
            Debug.Write("ViewPlan count:" + viewPlans.Count);

            return viewPlans.Select(o => new ViewPlanModel(o)).ToList();
        }
    }
}
