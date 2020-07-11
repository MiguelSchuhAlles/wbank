import React, { Component } from 'react';
  
class Deposit extends Component {
  state = {
    balance: 0
  }

  componentDidMount() {
    fetch('https://localhost:44337/api/accounts/3/balance',{
      headers: {
         authorization: "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzIiwidW5pcXVlX25hbWUiOiJEaW9nbyIsImVtYWlsIjoiZGlvZ29AZ21haWwuY29tIiwibmJmIjoxNTk0NDExODY2LCJleHAiOjE1OTQ0MTkwNjYsImlhdCI6MTU5NDQxMTg2Nn0.vYxrIdZdYv-aiTaji1QZFHd_tFNueilJj0p_1VQ_Or4"
       }
    })
    .then(res => res.json())
    .then((data) => {
      this.setState({ balance: data });
      console.log("Test " + data);
    })
    .catch(console.log)
  }

  render(){
    return (
        <form>
            <label>
            Nome:
            <input type="text" name="name" />
            </label>
            <input type="submit" value="Enviar" />
        </form>
    );
  }
} 

export default Deposit;
