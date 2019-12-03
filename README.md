<p align="center">
<img src="docs/_images/remember.png" alt="remember">
</p>
<h1 align="center">remember</h1>

> :blue_book: remember - 期望能帮到你

[![repo size](https://img.shields.io/github/repo-size/yiyungent/remember.svg?style=flat)]()
[![LICENSE](https://img.shields.io/github/license/yiyungent/remember.svg?style=flat)](https://github.com/yiyungent/remember/blob/master/LICENSE)



## 介绍

remember 是一个强大的内容管理系统，旨在让每个人都能轻松建站。

## 使用的第三方库

### 后端

- Entity Framework：ORM
- Autofac：Ioc容器
- AutoMapper：很方便的实现实体和实体之间的转换，一个强大的对象映射工具
- Log4Net：一个非常优秀的开源日志记录组件
- Newtonsoft.Json：Json序列化和反序列化工具
- StackExchange.Redis.StrongName：Redis 的.Net客户端之一

### 前端

- jQuery：一个 JavaScript 库，极大地简化了 JavaScript 编程
- bootstrap：来自 Twitter，是目前最受欢迎的前端框架
 
## Roadmap

- [x] 统一的异常处理（应用层几乎不需要自己写异常处理代码）
- [x] 数据有效性验证
- [x] 错误日志（自动记录程序异常）
- [x] 用户&角色管理
- [x] 插件系统（待完善）
  - [x] 设计为模块化和可扩展。提供构建自己的模块的基础结构。
  - [x] 每个插件都是一个完整MVC，创建您自己的模块生态系统，或重用现有模块来引导您的应用程序。
  - [x] 每个插件都可申请插件独有路由
  - [x] 插件可申请嵌入指定页面区域
  - [ ] 每个插件都可独有一张表
- [x] 模板系统
  - [x] 模板管理
  - [x] 允许用户切换模板（有此权限）
  - [x] ASP.NET MVC 5 Mobile 支持
  - [ ] 自定义 UA，不同UA使用不同主题模板
  - [ ] 风格管理（在模板管理的基础上，再增加对多风格的支持）
- [x] 自定义URL
- [x] Redis群集实现 Asp.net Mvc分布式Session
- [x] 用户默认生成唯一标识头像
- [x] 找回密码-邮件-验证码
- [x] 登录状态-效验-token
- [x] 站点设置
  - [x] 站点设置 缓存控制
- [ ] 自定义模板语言：静态页面生成（基于RazorEngine）
- [ ] OAuth2.0 开放授权支持
  - [ ] 搭建开放平台，用于第三方申请授权
- [ ] SSO（单点登录）
- [ ] 控制账户登录的单一性
- [ ] QQ登录（插件实现）
- [ ] 防水墙（频繁访问，操作等给与全局验证，拦截）
- [ ] 邮件提醒（Redis队列）
- [ ] CDN（内容分发网络）

### 功能点

- [x] 文章
  - [x] 管理中心-文章管理
  - [ ] SEO设置 - 文章 - 默认链接
  - [ ] SEO设置 - 文章 - title, description, keywords
  - [ ] 固定链接
  - [ ] 文章评论
  - [ ] 文章分类
  - [ ] 文章标签
- [x] 文库
  - [x] 管理中心-文库管理
  - [x] 一个文库有多个章节内容
  - [ ] 章节内容 支持发送弹幕
  - [ ] 文库评论
  - [ ] 文库分类
  - [ ] 文库标签
- [ ] 我的收藏
  - [x] 展示我的收藏夹
  - [x] 展示收藏夹内容项
  - [x] 使用收藏夹收藏文章，文库
  - [ ] 收藏夹管理（新建，编辑基本信息，删除）

## 快速开始



## 环境

- 运行环境: .NET Framework (>= 4.5)   
- 开发环境: Visual Studio Community 2019

## 相关项目


 
## 鸣谢

