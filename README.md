

# Bear.Core.Admin 项目 README

## 项目简介
本项目是基于 .NET 8 和 Vue 3.5+ 的前后端分离管理系统，采用现代化架构设计，包含权限管理、系统设置、数据操作等常见管理功能。

## 技术栈
- 后端：.NET 8, SqlSugar ORM
- 前端：Vue 3.5+, Typescript, Element Plus, alovajs
- 数据库：支持多种数据库，通过配置实现多租户支持
- 安全：JWT 认证, RSA 加密
- 其他：Redis 缓存, Quartz.NET 任务调度, Serilog 日志

## 主要功能模块
1. **权限管理**：包含用户、角色、部门、岗位等权限相关模块
2. **系统设置**：字典管理、参数配置、文件存储等系统设置功能
3. - **日志审计**：记录系统操作日志和异常日志
4. **邮件服务**：集成邮件账户管理和消息模板，支持定时邮件发送
5. **多租户支持**：通过数据库配置实现多租户架构
6. **动态表单**：支持自定义表单和表格视图配置
7. **定时任务**：集成 Quartz.NET 实现任务调度管理

## 项目结构
- **Bear.Core.Admin**：Vue 前端项目
- **Bear.Core.Api**：ASP.NET Core Web API 项目
- **Bear.Core.Business**：业务逻辑实现
- **Bear.Core.Common**：通用工具类和扩展方法
- **Bear.Core.Core**：核心功能，包括 AOP 拦截、系统配置等
- **Bear.Core.Entity**：数据库实体映射
- **Bear.Core.Infrastructure**：基础设施，包含过滤器、中间件等
- **Bear.Core.IBusiness**：业务接口定义
- **Bear.Core.Models**：数据传输对象(DTO)和视图模型
- **Bear.Core.Repository**：数据访问层
- **Bear.Core.TaskService**：定时任务服务
- **Bear.Core.ViewModel**：视图模型定义

## 开发规范
- 采用模块化开发，前后端功能对应清晰
- 使用 TypeScript 接口定义数据结构，提高类型安全性
- 组件化设计，支持快速搭建页面
- 完善的代码注释和文档说明
- 响应式布局，支持不同设备访问
- 前端使用 alova.js 实现优雅的 HTTP 请求处理

## 使用说明
1. 克隆仓库：`git clone https://gitee.com/ByteXiong/Bear.Core`
2. 还原后端依赖：`dotnet restore`
3. 还原前端依赖：`pnpm install`
4. 构建项目：`dotnet build` 或 `pnpm build`
5. 运行项目：`dotnet run` 或 `pnpm dev`

## 特性亮点
- 支持多语言：zh-cn, en-us �`
- 提供完善的代码生成和动态表单功能
- 集成 Redis 实现高性能缓存
- 支持响应式布局和主题切换
- 完善的异常处理和日志记录机制
- 支持动态路由和菜单配置

## 文档与支持
- 开发文档：[https://bear.js.org/](https://bear.js.org/)
- 前端框架：SoybeanAdmin(Element-plus) [文档](https://github.com/soybeanjs/soybean-admin-element-plus)
- 后端框架：ApoVolo [文档](https://gitee.com/xianhc/ape-volo-admin)

## 贡献指南
欢迎贡献代码和提出建议。请遵循以下原则：
1. Fork 项目并创建新分支
2. 确保代码风格一致，添加必要的注释
3. 测试通过后再提交 PR
4. 遵循项目编码规范和设计模式

## 许可协议
本项目采用 MIT 许可协议，详情请参阅 LICENSE 文件。