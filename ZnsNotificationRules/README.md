# ZnsNotificationRules

Thu vien cho cac loai tin ZNS cat/bam seal (PAYMENT, APPROVE, REJECT, COMPLETE, CANCEL placeholder).

## Cau truc

| Thu muc | Namespace | Vai tro | Pattern |
|---|---|---|---|
| `Contracts/` | `.Contracts` | `IZnsNotificationRule` (engine dung), `IZnsConfigReader`, `IZnsTransactionStore`, `IZnsGateway`, `IZnsRecipientResolver` | Strategy interface, Ports (DIP) |
| `Models/` | `.Models` | DTO, NOTIFY_TYPE, ten tham so, khoa Attributes, gioi han do dai, ma SYS_CODES, RunContext, ZnsQueueEntry, ZnsSendFlag, ZnsPendingMessage, ZnsOutboundMessage, ZnsSendResult | Value object |
| `Formatting/` | `.Formatting` | Dinh dang ngay, cat chuoi, ghep doi cong nhan | Utility |
| `Rules/Base/` | `.Rules.Base` | `ZnsNotificationRuleBase`: quy trinh co dinh (kiem tra, dung template, dung hang doi) | Template Method |
| `Rules/` | `.Rules` | Rule cu the: Payment, Approve, Reject, Complete, Cancel. Moi rule tu truy van va tong hop ho so rieng (`FetchPending`) | Concrete Strategy |
| `Engine/` | `.Engine` | `ZnsNotificationEngine`: Enqueue (ghi hang doi) va Dispatch (gui, danh dau) | Orchestrator |
| `Factory/` | `.Factory` | `ZnsRuleCatalog.Create()`: noi tao va dang ky rule | Factory / Registry |

Phu thuoc chi di mot chieu: `Engine` -> `Contracts`, `Models`; `Factory` -> `Rules` -> `Rules.Base` -> `Contracts`, `Models`, `Formatting`.

## Them template moi (vi du CANCEL)

Khong sua: `IZnsNotificationRule`, `ZnsNotificationRuleBase`, cac rule cu, DTO, `ZnsNotificationEngine`.

1. `Models/ZnsNotifyType.cs`: them hang NOTIFY_TYPE.
2. `Models/ZnsSealParamName.cs`: them ten tham so rieng.
3. `Models/ZnsTemplateLimits.cs`: them gioi han.
4. `Models/ZnsAttributeKeys.cs`: them khoa cho du lieu rieng (neu co).
5. `Rules/XxxZnsRule.cs`: ke thua `ZnsNotificationRuleBase`, goi `base(ZnsNotifyType.Xxx)`, override `Limits`, `SpecificParams`, `AddSpecificParams`, `FetchPending`. Override them `LoadConfig` khi can, `GetSpecificSkipReason` khi co validate rieng.
6. `Factory/ZnsRuleCatalog.cs`: them 1 dong.

## Engine va cac cong (port)

Job TOPO goi: `new ZnsNotificationEngine(ZnsRuleCatalog.Create(), config, recipients, store, gateway)` moi lan chay, roi:

1. `Enqueue(context)`: voi moi rule, `LoadConfig` -> `BuildPendingEntries` (trong do: `FetchPending` -> resolve phone -> `GetSkipReason` -> `BuildTemplateData` -> `GetMissingParams`) -> `IZnsTransactionStore.Save` (N hoac S).
2. `Dispatch(context)`: `IZnsTransactionStore.GetPending` -> `IZnsGateway.Send` -> `MarkSent` (Y) hoac `MarkFailed` (giu N, hoac F khi dat ZNSMAXRETRY).

Cac cong phia TOPO phai implement: `IZnsConfigReader`, `IZnsRecipientResolver`, `IZnsTransactionStore`, `IZnsGateway`. Truy van ho so khong dung cong chung: moi rule tu lam trong `FetchPending`.

## Luu y

- Rule khong duoc dang ky singleton: `LoadConfig` gan trang thai. `ZnsRuleCatalog.Create()` tao moi trong moi lan chay job.
- `FetchPending` hien tai tra du lieu gia lap. Khi tich hop, thay bang truy van DB cua tung loai tin.
- CANCEL hien la placeholder: ten tham so, gioi han va nguon du lieu la gia dinh, can xac nhan truoc khi dung.

## Build

Target `netstandard2.0`, C# 7.3. Dung duoc cho .NET Framework 4.6.1+ va .NET SDK.
