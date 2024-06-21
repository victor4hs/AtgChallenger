import * as React from 'react';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select, { SelectChangeEvent } from '@mui/material/Select';
import { Alert, AlertColor, Button, Card, CardHeader, FormControlLabel, FormLabel, Grid, Radio, RadioGroup, Snackbar, TextField } from '@mui/material';
import { validatePrice, validateAmnount, formatPrice } from './GlobalFunctions.tsx';
import { ProcessOrder } from './service/OrderGeneratorService.tsx';
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

const App = () => {

  const [connection, setConnection] = React.useState<null | HubConnection>(null);
  const [ notification, setNotification ] = 
    React.useState<{ message: string, openNotification: boolean, status: AlertColor }>(
      { message: "", openNotification: false, status: 'success'}
  );

  const handleClose = (event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }

    setNotification({ message: "", openNotification: false, status: 'success'});
  };

  React.useEffect(() => {
    const connect = new HubConnectionBuilder()
      .withUrl("http://localhost:51130/chatHub/notifications")
      .withAutomaticReconnect()
      .build();

    setConnection(connect);
  }, []);

  React.useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          connection.on("ReceiveMessage", (message, status) => {
            setNotification({ message: message, openNotification: true, status: status });
          });
        })
        .catch((error) => console.log(error));
    }
  }, [connection]);

  const [symbol, setSymbol] = React.useState('');

  
  const [orderType, setPurchaseType] = React.useState('');
  
  const [ammount, setAmmount] = React.useState<number>(1);
  
  const [price, setPrice] = React.useState<string>();
  const [errorPrice, setErrorPrice] = React.useState(false);

  const handleChangeSelect = (event: SelectChangeEvent) => {
    setSymbol(event.target.value);
  };

  const handleChangeRadio = (event: SelectChangeEvent) => {
    setPurchaseType(event.target.value);
  };

  const handleChangePrice = (event: any) => {
    setErrorPrice(!validatePrice(event.target.value));
    setPrice(formatPrice(event.target.value));
  };

  const handleChangeAmmount = (event: any) => {
    if(validateAmnount(event.target.value)){
      setAmmount(event.target.value);
    }
  };

  const handleSubmit = async () => {
    await ProcessOrder({
      symbol: symbol,
      orderType: orderType,
      price: price ?? "",
      ammount: ammount.toString()
    });
  }

  const isFormValid = (): boolean => {
    if(symbol === ''){
      return false;
    }
    if(orderType === ''){
      return false;
    }
    if(!validateAmnount(ammount)){
      return false;
    }
    if(!validatePrice(price?.toString() ?? "")){
      return false;
    }
    return true;
  }

  return (
      <Grid container justifyContent="center" spacing={2}>
        <Snackbar open={notification.openNotification} autoHideDuration={6000} onClose={handleClose}>
          <Alert
            onClose={handleClose}
            severity="success"
            variant="filled"
            sx={{ width: '100%' }}
          >
            { notification.message }
          </Alert>
      </Snackbar>
        <Grid item md={6}>
          <Card style={{padding: '3%'}}>
            <CardHeader title="Compra e venda de ações"></CardHeader>
            <Grid item style={{padding: '3%'}} xs={12}>
              <FormControl fullWidth>
                <InputLabel>Símbolo</InputLabel>
                <Select
                  id="symbol"
                  value={symbol}
                  onChange={handleChangeSelect}
                  label="Símbolo"
                >
                  <MenuItem value={'PETR4'}>PETR4</MenuItem>
                  <MenuItem value={'VALE3'}>VALE3</MenuItem>
                  <MenuItem value={'VIIA4'}>VIIA4</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid item style={{padding: '3%'}} xs={12}>
              <FormControl fullWidth>
                <FormLabel>Tipo</FormLabel>
                <RadioGroup
                  row
                  name="order"
                  value={orderType}
                  onChange={handleChangeRadio}
                >
                  <FormControlLabel value="buy" control={<Radio />} label="Compra" />
                  <FormControlLabel value="sell" control={<Radio />} label="Venda" />
                </RadioGroup>
              </FormControl>
            </Grid>
            <Grid item style={{padding: '3%'}} xs={12}>
              <FormControl fullWidth>
                <TextField
                  id="ammount"
                  label="Quantidade"
                  type="number"
                  InputLabelProps={{
                    shrink: true,
                  }}
                  onChange={handleChangeAmmount}
                  value={ammount}
                  fullWidth
                />
              </FormControl>
            </Grid>
            <Grid item style={{padding: '3%'}} xs={12}>
              <FormControl fullWidth>
                <TextField
                  error={errorPrice}
                  id="price"
                  label="Preço"
                  onChange={handleChangePrice}
                  helperText={errorPrice ? "Preço inválido (Precisa ser maior que 0 e menor que 1000)" : ""}
                  value={price}
                  fullWidth
                />
              </FormControl>
            </Grid>
            <Grid item style={{padding: '3%'}} >
              <Button 
                onClick={handleSubmit} 
                type="submit" 
                fullWidth 
                variant="contained"
                disabled={!isFormValid()}
              >
                Enviar
              </Button>
            </Grid>
          </Card>
        </Grid>
        
      </Grid>
  );
}

export default App;