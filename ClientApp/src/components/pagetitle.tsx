import React from 'react'
interface Props{children:React.ReactNode}
export const PageTitle =({children}:Props)=>{
return (
    <div>
            <h5>pagetitle</h5>
      <h2 style={{"backgroundColor":"red"}}> {children}</h2> 

    </div>
)
}