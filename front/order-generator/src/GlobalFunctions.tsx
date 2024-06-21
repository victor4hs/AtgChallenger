export const validatePrice = (value: string): boolean => {
    value = value.replace(/\D/g, "");
    const newValue = parseFloat(value) / 100;

    if (Math.round(newValue * 100) / 100 !== newValue) {
        return false;
    }
    if (newValue <= 0) {
        return false;
    }
    if (newValue >= 1000) {
        return false;
    }
    return !isNaN(newValue);
}

export const validateAmnount = (value: number): boolean => {
    if (value <= 0) {
        return false;
    }
    if (value > 100000) {
        return false;
    }
    return !isNaN(value);
}

export const formatPrice = (value: string): string => {
    console.log(value);
    console.log('value');
    if(!value || value.trim() === "R$")
        return "";
    
    value = value.replace(/\D/g, "");
    let valueFloat = parseFloat(value) / 100;
    return "R$ " + valueFloat.toLocaleString('pt-BR', { minimumFractionDigits: 2 });
}