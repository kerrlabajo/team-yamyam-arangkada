for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "4rAngKada!*" -d master -i "arangkadadb-schema.sql"
    if [ $? -eq 0 ]
    then
        echo "Arangkada imported" && pkill sqlservr 
        sleep 10
        break
    else
        echo "Loading..."
        sleep 1
    fi
done

# restart SQL Server in the foreground
/opt/mssql/bin/sqlservr