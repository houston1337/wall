#!/bin/bash
# Укажите путь к файлу
file_path="my_git.txt"

# Укажите ключ, который вы ищете
desired_key=$(date +"%j")

# Поиск значения по ключу
value=$(grep "^$desired_key=" "$file_path" | cut -d '=' -f2)

# Проверка наличия значения
if [ -n "$value" ]; then
    echo "Значение для ключа '$desired_key': $value"
else
    echo "Ключ '$desired_key' не найден в файле."
fi

for i in $(seq 1 $value)
do
	echo -e "day $desired_key \ncommit $i/$value">1.txt
	git add 1.txt
	git commit -m "commit $i/$value for day $desired_key"
done
	
