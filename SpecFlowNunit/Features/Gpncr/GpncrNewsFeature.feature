Функция: Отображение списка новостой на странице

@Severity:Critical
Сценарий: Список всех новостей. Веб-часть отображается на странице
	#Дано пользователь нашел сnраницу в google
	Пусть пользователь открыл страницу "https://ds.gazprom-neft.ru/"
	#Новости (страница)
	Тогда "//div[@data-href='sg-markup-patterns-tile-list-news-html']" присутствует на странице
	#//div[@data-href='sg-markup-patterns-tile-list-news-html']