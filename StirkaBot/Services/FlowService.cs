using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StirkaBot.Models;
using Newtonsoft.Json.Linq;

namespace StirkaBot.Services
{
    public class FlowService
    {
        public const string COLOR_PRIMARY = "primary";
        public const string COLOR_SECONDARY = "secondary";
        public const string COLOR_POSITIVE = "positive";
        public const string COLOR_NEGATIVE = "negative";

        private NLog.Logger _logger;

        public FlowService(NLog.Logger logger)
        {
            _logger = logger;
        }

        public Flow initFlow()
        {
            var flow = new Flow();

            var start = new Flow.Node() { label = "" };
            var goal = new Flow.Node() { label = "Цель визита" };
            var goalInput = new Flow.Node() { label = "Ввидите цель визита" };
            var cleanliness = new Flow.Node() { label = "Чистота в прачечной" };
            var cleanlinessPhoto = new Flow.Node() { label = "Пришлите фотографию" };
            var commonPhoto = new Flow.Node() { label = "Общее фото объекта" };
            var issues = new Flow.Node() { label = "Осуществлен ремонт/устранение проблем" };
            var issuesNumberInput = new Flow.Node() { label = "Введите номер машины" };
            var issuesWorkInput = new Flow.Node() { label = "Напишите что было сделано" };
            var issuesInput = new Flow.Node() { label = "Опишите проблему" };
            var collection = new Flow.Node() { label = "Была ли инкассация" };
            var collectionPhoto = new Flow.Node() { label = "Пришлите фотографию" };
            var end = new Flow.Node() { label = "Окончание визита" };

            start.add(getStartLink(goal));

            goal.add(new Flow.Link() { label = "Поломка", node = cleanliness })
                .add(new Flow.Link() { label = "Плановый визит", node = cleanliness })
                .add(new Flow.Link() { label = "Инкассация", node = cleanliness })
                .add(getOtherLink(goalInput));

            goalInput.add(getNextLink(goal));

            cleanliness.add(new Flow.Link() { label = "Чисто", node = commonPhoto })
                .add(new Flow.Link() { label = "Норм", node = commonPhoto })
                .add(new Flow.Link() { label = "Грязно", node = cleanlinessPhoto });

            cleanlinessPhoto.add(getNextLink(commonPhoto));

            commonPhoto.add(getNextLink(issues));

            issues.add(new Flow.Link() { label = "Номер машины", node = issuesNumberInput })
                .add(new Flow.Link() { label = "Помпа", node = issues })
                .add(new Flow.Link() { label = "Щетки", node = issues })
                .add(new Flow.Link() { label = "Резинка дверцы", node = issues })
                .add(new Flow.Link() { label = "Шланг", node = issues })
                .add(new Flow.Link() { label = "Бункер порошка", node = issues })
                .add(new Flow.Link() { label = "Кнопка залипла", node = issues })
                .add(new Flow.Link() { label = "Планшет", node = issues })
                .add(new Flow.Link() { label = "Зажевало купюру", node = issues })
                .add(new Flow.Link() { label = "Неисправен монетник", node = issues })
                .add(new Flow.Link() { label = "Фильтр", node = issues })
                .add(new Flow.Link() { label = "Датчик", node = issues })
                .add(getOtherLink(issuesInput))
                .add(getNextLink(collection));

            issuesNumberInput.add(getNextLink(issuesWorkInput));
            issuesWorkInput.add(getNextLink(issues));

            issuesInput.add(getNextLink(issues));

            collection.add(new Flow.Link() { label = "Да", node = collectionPhoto, color = COLOR_POSITIVE })
                .add(new Flow.Link() { label = "Нет", node = end, color = COLOR_NEGATIVE });

            collectionPhoto.add(getNextLink(end));

            end.add(getStartLink(goal));

            flow.add(new List<Flow.Node>() { 
                start,
                goal,
                goalInput,
                cleanliness,
                cleanlinessPhoto,
                commonPhoto,
                issues,
                issuesNumberInput,
                issuesWorkInput,
                issuesInput,
                collection,
                collectionPhoto,
                end
            });
            return flow;
        }

        public static Flow.Link getNextLink(Flow.Node node)
        {
            return new Flow.Link() { label = "Продолжить", node = node, color = COLOR_POSITIVE };
        }

        public static Flow.Link getOtherLink(Flow.Node node)
        {
            return new Flow.Link() { label = "Другое", node = node, color = COLOR_PRIMARY };
        }

        public static Flow.Link getStartLink(Flow.Node node)
        {
            return new Flow.Link() { label = "Начать", node = node, color = COLOR_PRIMARY };
        }

        public static string convertToKeyboard(Flow.Node node)
        {
            //convert to buttons
            var buttons = node.links.Select(t =>
                new VKBot.Models.MenuButton()
                {
                    action = new VKBot.Models.MenuAction()
                    {
                        label = t.Value.label,
                        payload = JObject.FromObject(new
                        {
                            node = node.id,
                            link = t.Value.id,
                            label = t.Value.label
                        }).ToString(),
                        type = "text",
                    },
                    color = t.Value.color
                }
            ).ToList();
            
            //split by chunks(max button rows amount = 10)
            int chunkSize = (int)Math.Ceiling((decimal)buttons.Count / 10);

            var buttonChunks = new List<List<VKBot.Models.MenuButton>>();
            for (int i = 0; i < buttons.Count; i += chunkSize)
            {
                buttonChunks.Add(buttons.GetRange(i, Math.Min(chunkSize, buttons.Count - i)));
            }

            //convert to keyboard json
            return JObject.FromObject(new
            {
                one_time = false,
                buttons = buttonChunks
            }).ToString();
        }
    }
}
