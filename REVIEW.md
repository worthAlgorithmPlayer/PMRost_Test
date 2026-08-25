# Backend

.FirstOrDefaultAsync().Result; - синхоронное ожидание, убираем .Result добавляем await, не предпочтительное обращение к базе синхронно, неочевидное поведение и deadlock !!!

Запрос entries выгружает всю коллекцию в память, далее мы проходимся по всей (огромной) коллекции и находим нужные нам monthEntries. (Фильтруем обязательно сразу в бд) !!

var employee = \_db.GetCollection<Employee>("employees")... - находится в цикле, таким образом мы сделаем n запросов
в базу для получения каждого employee, нагрузка на базу и endpoint будет работать медленно (Нужно делать batch запросы) !!

var project = await \_db.GetCollection<Project>("projects") - тоже самое, делаем запросы по каждому проекту в цикле, но здесь есть уже await !!

var rate = employee.Rates.FirstOrDefault().Value; - берётся первая попавшаяся !!

Нигде нет проверок на employee == null -> throw, project != null ..., rate != null !!

row.Amount / row.Budget - бюджет вряд ли будет 0, но проверку нужно т.к. иначе будет ошибка !!

public double Budget { get; set; } - всё что про деньги нужно хранить в decimal, так как иначе возникают проблемы с плавающей запятой и расчёт будет неверен !!

CancellationToken token - вроде есть, можно применить в .ToListAsync(token) произвести отмену, если потребуется возможности не будет

using MongoDB.Driver; - в некоторых случаях потребуется using MongoDB.Driver.linq; (не выделил как важный, так как сразу будут ошибки)

## Косметика или спорно:

GetProjectReportQuery, TimesheetReportHandler - я бы делал GetProjectReportQuery GetProjectReportQueryHandler явная ошибка в названии, в стилистике Query, QueryHandler может необязательно

ProjectReportRow назвал бы ProjectReportRowModel

public string Comment { get; set; } - скорее всего комментарий необязательный - nullable

## Структура кода

Добавил бы базовые классы для сущностей: DomainEntity, AggregateRoot ...
Использование \_db напрямую для Read запросов, можно делать Select нужных полей, таким образом снижая нагрузку

# Front

useEffect(() => {
load();
}); - требуются условия вызова, иначе может возникнуть цикл и мы даже не сможем загрузить страницу

entries.push(body); setEntries(entries); - либо обновляем страницу, либо получаем ответ от сервера и добавляет его

e.employeeId == employeeId - делаем строгое сравнение ===

method: "PUT" для создания не используется, должен быть в Post, а вот наоборот обновление может быть и Post

const [hours, setHours] = useState("") - используем число, а не строку

alert("Сохранено"); - вряд ли используется в текущих реалиях

## Структура кода

Использовал бы FSD
