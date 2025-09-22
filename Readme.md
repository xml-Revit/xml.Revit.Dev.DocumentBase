# xml.Revit.Dev.DocumentBase

Autodesk Revit插件项目，支持Revit 2020-2026版本，实现高版本文件降级到低版本的功能。

## 项目功能

这个插件主要实现Revit高版本文件降级到低版本的功能。通过导出高版本Revit文件中的数据，然后在低版本Revit中导入这些数据，实现跨版本的数据迁移，解决了Revit版本不兼容的问题。

### 当前支持的元素类型

* 标高 (Levels)
* 轴网 (Grids)
* 楼层平面视图 (ViewPlans)
* 墙体 (Walls)

### 工作流程

1. 在高版本Revit中使用"导出数据"命令，将项目数据导出为.db文件
2. 在低版本Revit中使用"导入数据"命令，从.db文件导入数据到新项目中

## 详细使用指南

### 安装插件

1. 下载并安装对应Revit版本的插件
2. 重启Revit，插件将自动加载
3. 在Revit功能区中找到"xml.Revit.Dev.DocumentBase"选项卡

### 导出数据（高版本Revit）

1. 打开需要降级的Revit项目文件
2. 点击"导出数据"命令
3. 选择保存位置，系统会生成一个.db格式的数据文件
4. 导出完成后会显示导出的元素数量

### 导入数据（低版本Revit）

1. 在低版本Revit中创建新项目
2. 点击"导入数据"命令
3. 选择之前导出的.db文件
4. 系统会自动在低版本项目中重建相应的元素
5. 导入完成后会显示导入的元素数量

## 如何新增类型功能

如果您想为项目贡献新的元素类型支持，可以按照以下步骤进行：

### 1. 创建模型类

在`Models`文件夹中创建新的模型类，继承自`ElementModel`基类：

```csharp
public sealed class YourElementModel : ElementModel
{
    // 构造函数，从Revit元素读取数据
    public YourElementModel(Element element)
    {
        // 读取元素属性并赋值
        UniqueId = element.UniqueId;
        Name = element.Name;
        // 其他特定属性...
    }
    
    // 定义该元素类型特有的属性
    public string Property1 { get; set; }
    public double Property2 { get; set; }
    // ...
}
```

### 2. 创建服务接口

在`Services`文件夹中定义服务接口：

```csharp
public interface IYourElementService : IElementService<YourElementModel>
{
    // 可以添加特定于该元素类型的方法
}
```

### 3. 实现服务类

创建服务实现类：

```csharp
public sealed class YourElementService : ElementService<YourElementModel>, IYourElementService
{
    // 实现从文档获取元素模型的方法
    public override IList<YourElementModel> GetElementModels()
    {
        var result = new List<YourElementModel>();
        // 从当前文档获取元素并转换为模型
        var elements = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.YOUR_CATEGORY)
            .WhereElementIsNotElementType()
            .ToElements();
            
        foreach (var element in elements)
        {
            result.Add(new YourElementModel(element));
        }
        
        return result;
    }
    
    // 实现创建元素的方法
    public override IList<Element> Create(IList<YourElementModel> models)
    {
        var result = new List<Element>();
        // 根据模型创建Revit元素
        foreach (var model in models)
        {
            // 创建元素的代码...
            // 将创建的元素添加到结果列表
        }
        
        return result;
    }
}
```

### 4. 注册服务

在`Host.cs`文件中注册您的服务：

```csharp
services.AddSingleton<IYourElementService, YourElementService>();
```

### 5. 更新导入/导出命令

在`CmdExport.cs`和`CmdImport.cs`中添加对新元素类型的支持：

```csharp
// 在导出命令中
IYourElementService yourElementService = Host.GetService<IYourElementService>();
var yourElementModels = yourElementService.GetElementModels();
LiteDBHelper.Upsert(db, yourElementModels);

// 在导入命令中
var yourElementModels = LiteDBHelper.Query<YourElementModel>(db);
IYourElementService yourElementService = Host.GetService<IYourElementService>();
doc.Transaction(t => yourElementService.Create(yourElementModels));
```

## 参与贡献

该项目已开源，欢迎提交Issue以支持更多元素类型。您可以通过以下方式参与：

* 提交新的元素类型支持
* 改进现有功能
* 修复问题和bug
* 提供使用反馈和建议

## 技术栈

* C# 12
* .NET Framework 4.8
* .NET 8
* LiteDB (用于数据存储)

## 开发环境准备

Before you can build this project, you will need to install .NET, depending upon the solution file you are building. If you haven't already installed these
frameworks, you can do so by visiting the following:

* [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet)


## 构建项目

   编译:
   ```powershell
   nuke
   ```

   创建安装程序:
   ```powershell
   nuke CreateInstaller
   ```

## 联系方式

* 微信公众号: Revit二次开发教程
* 邮箱: zedmoster1@gmail.com
