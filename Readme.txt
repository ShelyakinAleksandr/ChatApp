1) Скачать и установить .net 8
2) Скачать и установить Docker
3) Скачать проект
4) Командой "ipconfig" скопировать свой IPv4-адрес
5) В проекте открыть файл appsettings.Staging.json
6) В файле заменить ip Бд (это Host) на скопированный из шага 4. Строка подключения к бд должна иметь:
	"Host={IPv4-адрес};Port=5432;Database=ChatDb;Username=postgres;Password=123456;Include Error Detail=true"
   Например:
	"Host=192.168.1.100;Port=5432;Database=ChatDb;Username=postgres;Password=123456;Include Error Detail=true"
7) Создать в папке проекта, папку "publish"
8) В консоли перейти в папку проекта
8) В консоли выполнить команду:
	dotnet publish -c Release -o publish ChatApp
9) В консоли выполнить команду:
	docker compose up -d --build
10) В консоли выполнить команду:
	dotnet ef database update --project Infrastructure --startup-project ChatApp
11) Открыть в браузере ссылку:
	http://localhost:5050/swagger/index.html
