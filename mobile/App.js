import { useState } from 'react';
import { View, Text, TextInput, Pressable, Alert } from 'react-native';
import { api } from './src/services/api';

export default function App() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  async function handleLogin() {
    try {
      const response = await api.post('/auth/login', {
        email,
        password
      });

      console.log(response.data);
      Alert.alert('Sucesso', 'Login realizado');
    } catch (error) {
      console.log(error.response?.data);
      Alert.alert('Erro', 'Falha no login');
    }
  }

  return (
    <View style={{ padding: 20, marginTop: 100 }}>
      <Text>Email</Text>
      <TextInput
        value={email}
        onChangeText={setEmail}
        style={{ borderWidth: 1, marginBottom: 10, borderRadius: 10 }}
      />

      <Text> Senha</Text>
      <TextInput
        value={password}
        onChangeText={setPassword}
        secureTextEntry
        style={{ borderWidth: 1, marginBottom: 20, borderRadius: 10 }}
      />

      <Pressable
        onPress={handleLogin}
        style={{ backgroundColor: 'black', padding: 10, borderRadius: 20 }}
      >
        <Text style={{ color: 'white', textAlign: 'center' }}>
          Entrar
        </Text>
      </Pressable>
    </View>
  );
}