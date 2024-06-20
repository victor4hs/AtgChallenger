# Atg Challenger

O desafio se propõem a fazer um sistema de venda e compra de algumas ações usando o protocolo FIX (https://quickfixn.org/) em .NET

O front foi escolhido o ReactJS

O back foi criado duas aplicações uma sendo o OrderAccumulator responsável por receber as mensagens da API e calcular a exposição financeira e tem o OrderGeneratorAPI sendo a API que recebe as solicitações do front e manda para o OrderAccumulator calcular a exposição, caso tudo ocorra bem responde com sucesso se não responde exibindo um erro ao calcular a ordem.

Para websocket de comunicação com o front usei o SignalR
